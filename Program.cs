
using MARN_API.Data;
using MARN_API.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using MARN_API.Services.Implementations;
using MARN_API.Configurations;
using MARN_API.Mapping;
using MARN_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using MARN_API.Repositories.Interfaces;
using MARN_API.Repositories.Implementations;
using MARN_API.Repositories;
using MARN_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MARN_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add User Secrets in Development environment
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }


            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddFile("Logs/app-{Date}.txt");
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddEventSourceLogger();
            }
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();

                    logger.LogWarning("Model validation failed");

                    return new BadRequestObjectResult(context.ModelState);
                };
            });


            // Configure file upload size limits
            builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
            });


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Include XML comments
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // Add JWT Bearer authentication to Swagger
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            // Dependency Injection
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Repos
            builder.Services.AddScoped<IBookingRequestRepo, BookingRequestRepo>();
            builder.Services.AddScoped<IContractRepo, ContractRepo>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
            builder.Services.AddScoped<IRoommatePreferenceRepo, RoommatePreferenceRepo>();
            builder.Services.AddScoped<ISavedPropertyRepo, SavedPropertyRepo>();
            // Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddSignalR();

            builder.Services.AddSingleton<Microsoft.AspNetCore.SignalR.IUserIdProvider, MARN_API.Hubs.CustomUserIdProvider>();
            builder.Services.AddSingleton<MARN_API.Hubs.ConnectionTracker>();

            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();

        

            // Health Checks
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("database")
                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "self" });


            // Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                option.SignIn.RequireConfirmedEmail = true;
                // option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // 15 minutes
                option.Lockout.MaxFailedAccessAttempts = 5; // lockout after 5 invalid attempts
                option.Lockout.AllowedForNewUsers = true;  // lockout enabled for all new users
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                // Default provider (used for email confirmation & password reset)
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });

            // Configure the email token provider specifically
            builder.Services.Configure<DataProtectionTokenProviderOptions>(TokenOptions.DefaultEmailProvider, opt =>
            {
                // 2FA codes expire quickly
                opt.TokenLifespan = TimeSpan.FromMinutes(5);
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireMfa", policy =>
                {
                    policy.RequireClaim("amr", "mfa");
                });
            });


            // CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin => true) // Allow any origin during development
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Required for SignalR
                });
            });
            // If you want to allow only a specific domain, you can use the following code instead:
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowCustomDomain",
            //        builder => builder.WithOrigins("http://127.0.0.1:5500")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});


            // Authentication and Authorization
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "CoolAuthentication";
                options.DefaultChallengeScheme = "CoolAuthentication";
            })
            .AddJwtBearer("CoolAuthentication", options =>
            {
                var secretKeyString = builder.Configuration.GetValue<string>("Jwt:SecretKey");
                var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
                var audience = builder.Configuration.GetValue<string>("Jwt:Audience");
                var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString!);
                var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                };

                // Add SignalR authentication handling for WebSocket connections
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        // Extract query string token if request is destined for the SignalR hub
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));


            // Add AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Rate Limiting
            builder.Services.AddRateLimiter(options =>
            {
                // Global rate limiting policy
                options.GlobalLimiter = System.Threading.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                            QueueLimit = 10
                        }));

                // Strict policy for authentication endpoints
                options.AddPolicy("StrictAuth", context =>
                    System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                // Moderate policy for general API endpoints
                options.AddPolicy("Moderate", context =>
                    System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                            QueueLimit = 5
                        }));

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync(
                        "Rate limit exceeded. Please try again later.", cancellationToken);
                };
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Global exception handling should be registered BEFORE request logging
            // so exceptions are caught before the logging middleware tries to process the response
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


            // Request logging
            app.UseMiddleware<RequestLoggingMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Rate limiting should be applied before authentication
            app.UseRateLimiter();


            app.UseCors();
            // If you want to allow only a specific domain, you can use the following code instead:
            //app.UseCors("AllowCustomDomain");


            app.UseAuthentication();
            app.UseAuthorization();




            // Health Check endpoints
            app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true
            });

            app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("database") || check.Tags.Contains("self")
            });

            app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("self")
            });


            app.MapControllers();
            app.MapHub<MARN_API.Hubs.ChatHub>("/chatHub");

            await app.RunAsync();
        }
    }
}
