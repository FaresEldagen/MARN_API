using MARN_API.Configurations;
using MARN_API.Data;
using MARN_API.Middleware;
using MARN_API.Models;
using MARN_API.Repositories.Implementations;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Implementations;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.RateLimiting;
using MARN_API.Hubs;

namespace MARN_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Logging Configuration
            // Configure application logging (Console, Debug, File)
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddFile("Logs/app-{Date}.txt");

            if (builder.Environment.IsDevelopment())
                builder.Logging.AddEventSourceLogger();

            // Custom model validation response
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
            #endregion


            #region Request & Upload Limits
            // Limit file upload size (10MB)
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
            });
            #endregion


            #region Controllers & JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Convert Enums to string instead of int
                    options.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            #endregion


            #region Swagger Configuration
            builder.Services.AddSwaggerGen(options =>
            {
                // XML Documentation
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);

                // JWT Support
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Enter: Bearer {your token}",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
            #endregion


            #region Database Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion


            #region Dependency Injection
            // Repositories
            builder.Services.AddScoped<IBookingRequestRepo, BookingRequestRepo>();
            builder.Services.AddScoped<IContractRepo, ContractRepo>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
            builder.Services.AddScoped<IRoommatePreferenceRepo, RoommatePreferenceRepo>();
            builder.Services.AddScoped<ISavedPropertyRepo, SavedPropertyRepo>();
            builder.Services.AddScoped<IReportRepo, ReportRepo>();
            builder.Services.AddScoped<IReviewRepo, ReviewRepo>();

            // Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IChatRepo, ChatRepo>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();
            #endregion


            #region SignalR Configuration
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserIdProvider, MARN_API.Hubs.CustomUserIdProvider>();
            builder.Services.AddSingleton<MARN_API.Hubs.ConnectionTracker>();
            #endregion


            #region Health Checks
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("database")
                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
            #endregion


            #region Identity Configuration
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

            // Token Lifetimes
            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                // Default provider (used for email confirmation & password reset)
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(TokenOptions.DefaultEmailProvider, opt =>
            {
                // 2FA codes expire quickly
                opt.TokenLifespan = TimeSpan.FromMinutes(5);
            });

            // MFA Policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireMfa", policy =>
                {
                    policy.RequireClaim("amr", "mfa");
                });
            });
            #endregion


            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowCustomDomain",
            //        policy => policy.WithOrigins(builder.Configuration["AppSettings:FrontBaseUrl"]!)
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials());
            //});
            #endregion


            #region Authentication (JWT)
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

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("Jwt"));
            #endregion


            #region AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion


            #region Rate Limiting
            builder.Services.AddRateLimiter(options =>
            {
                string GetKey(HttpContext context) =>
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                FixedWindowRateLimiterOptions CreateOptions(int permit, int queue = 0) =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permit,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = queue
                    };

                // Global
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetKey(context),
                        _ => CreateOptions(100, 10)));

                // Strict (Auth)
                options.AddPolicy("StrictAuth", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetKey(context),
                        _ => CreateOptions(5)));

                // Moderate
                options.AddPolicy("Moderate", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetKey(context),
                        _ => CreateOptions(30, 5)));

                // On Rejected
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync(
                        "Rate limit exceeded. Please try again later.", token);
                };
            });
            #endregion


            var app = builder.Build();


            #region Middleware Pipeline
            // Global Exception Handling
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // Logging Requests
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRateLimiter();

            app.UseCors();
            //app.UseCors(builder.Configuration["AppSettings:FrontBaseUrl"]!);

            app.UseAuthentication();
            app.UseAuthorization();
            #endregion


            #region Endpoints
            app.MapControllers();

            // SignalR Hub
            app.MapHub<ChatHub>("/hubs/chat");
            app.MapHub<NotificationHub>("/hubs/notification");

            // Health Checks
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/health/ready");
            app.MapHealthChecks("/health/live");
            #endregion


            await app.RunAsync();
        }
    }
}