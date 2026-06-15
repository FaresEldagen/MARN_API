using MARN_API.Configurations;
using MARN_API.Data;
using MARN_API.Filters;
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
using MARN_API.Localization;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;
using System.Text.Json;
using Hangfire;
using Hangfire.SqlServer;
using MARN_API.BackgroundJobs;

namespace MARN_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddHttpContextAccessor();


            #region Logging Configuration
            // Configure application logging (Console, Debug, File)
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddFile("Logs/app-{Date}.txt");

            if (builder.Environment.IsDevelopment())
                builder.Logging.AddEventSourceLogger();

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
            var mvcBuilder = builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<BannedAccountAccessFilter>();
                })
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options =>
                {
                    // Convert Enums to string instead of int
                    options.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });

            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();
                    var localizer = context.HttpContext.RequestServices
                        .GetRequiredService<IAppTextLocalizer>();

                    logger.LogWarning("Model validation failed for path {Path}", context.HttpContext.Request.Path);

                    var errors = context.ModelState
                        .Where(entry => entry.Value?.Errors.Count > 0)
                        .ToDictionary(
                            entry => NormalizeValidationKey(entry.Key),
                            entry => entry.Value!.Errors
                                .Select(error =>
                                {
                                    var message = string.IsNullOrWhiteSpace(error.ErrorMessage)
                                        ? "The provided value is invalid."
                                        : error.ErrorMessage;
                                    return ValidationMessageLocalizer.Localize(entry.Key, message, localizer);
                                })
                                .ToArray());

                    return new BadRequestObjectResult(new ErrorResponse
                    {
                        Code = "VALIDATION_FAILED",
                        Message = localizer.LocalizeMessage("VALIDATION_FAILED", "The request payload is invalid."),
                        StatusCode = StatusCodes.Status400BadRequest,
                        Path = context.HttpContext.Request.Path,
                        TraceId = context.HttpContext.TraceIdentifier,
                        Timestamp = DateTime.UtcNow,
                        Errors = errors
                    });
                };
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

                options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
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
            builder.Services.AddScoped<IPropertyFeedbackRepo, PropertyFeedbackRepo>();
            builder.Services.AddScoped<IPropertyAmenityRepo, PropertyAmenityRepo>();
            builder.Services.AddScoped<IPropertyMediaRepo, PropertyMediaRepo>();
            builder.Services.AddScoped<IPropertyRuleRepo, PropertyRuleRepo>();
            builder.Services.AddScoped<IRoommatePreferenceRepo, RoommatePreferenceRepo>();
            builder.Services.AddScoped<ISavedPropertyRepo, SavedPropertyRepo>();
            builder.Services.AddScoped<IReportRepo, ReportRepo>();
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IAdminDashboardRepo, AdminDashboardRepo>();
            builder.Services.AddScoped<IAdminAnalyticsReportRepo, AdminAnalyticsReportRepo>();
            builder.Services.AddScoped<IAdminDetailedStatsRepo, AdminDetailedStatsRepo>();
            builder.Services.AddScoped<IAdminRoleManagementRepo, AdminRoleManagementRepo>();
            builder.Services.AddScoped<IAdminVerificationRepo, AdminVerificationRepo>();
            builder.Services.AddScoped<IAdminUserManagementRepo, AdminUserManagementRepo>();
            builder.Services.AddScoped<IAssistantChatRepo, AssistantChatRepo>();

            // Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            builder.Services.AddScoped<IAdminAnalyticsReportService, AdminAnalyticsReportService>();
            builder.Services.AddScoped<IAdminDetailedStatsService, AdminDetailedStatsService>();
            builder.Services.AddScoped<IAdminRoleManagementService, AdminRoleManagementService>();
            builder.Services.AddScoped<IAdminReportModerationService, AdminReportModerationService>();
            builder.Services.AddScoped<IAdminVerificationService, AdminVerificationService>();
            builder.Services.AddScoped<IAdminUserManagementService, AdminUserManagementService>();
            builder.Services.AddScoped<IContractService, ContractService>();
            builder.Services.AddScoped<IContactSupportService, ContactSupportService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IPropertyFeedbackService, PropertyFeedbackService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IContractDocumentStorage, ContractDocumentStorage>();
            builder.Services.AddScoped<IRoommateMatchingService, RoommateMatchingService>();
            builder.Services.AddScoped<IChatRepo, ChatRepo>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOwnerService,OwnerService>();
            builder.Services.AddScoped<IBookingRequestService, BookingRequestService>();
            builder.Services.AddScoped<IHomepageService, HomepageService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IAppTextLocalizer, AppTextLocalizer>();
            builder.Services.AddScoped<IResponsePayloadLocalizer, ResponsePayloadLocalizer>();
            builder.Services.AddScoped<IUserCultureService, UserCultureService>();
            builder.Services.AddScoped<INotificationContentLocalizer, NotificationContentLocalizer>();
            builder.Services.AddScoped<IUserActivityService, UserActivityService>();
            builder.Services.AddScoped<IAssistantChatService, AssistantChatService>();

            builder.Services.AddScoped<IContractPdfGenerator, ContractPdfGenerator>();
            builder.Services.AddScoped<IHashingService, HashingService>();
            builder.Services.AddScoped<IOpenTimestampsProofReader, OpenTimestampsProofReader>();
            builder.Services.AddHttpClient<IOpenTimestampsService, OpenTimestampsService>();
            builder.Services.AddHttpClient<ICurrencyExchangeService, CurrencyExchangeService>();
            builder.Services.AddHttpClient<IExternalPropertyAiClient, ExternalPropertyAiClient>();
            builder.Services.AddHttpClient<IAssistantAiClient, AssistantAiClient>();

            builder.Services.AddScoped<PaymentScheduleJob>();
            builder.Services.AddScoped<PaymentJob>();
            builder.Services.AddScoped<OtsUpgradeJob>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();
            #endregion


            #region Stripe Configuration
            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            #endregion


            #region SignalR Configuration
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserIdProvider, MARN_API.Hubs.CustomUserIdProvider>();
            builder.Services.AddSingleton<MARN_API.Hubs.ConnectionTracker>();
            #endregion


            #region Hangfire Configuration
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            builder.Services.AddHangfireServer();
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
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        if (context.Response.HasStarted)
                            return;

                        context.HandleResponse();

                        var localizer = context.HttpContext.RequestServices.GetRequiredService<IAppTextLocalizer>();
                        var (code, fallbackMessage) = ResolveAuthenticationChallenge(context);
                        var response = CreateFrameworkErrorResponse(context.HttpContext, localizer, StatusCodes.Status401Unauthorized, code, fallbackMessage);

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions), context.HttpContext.RequestAborted);
                    },
                    OnForbidden = async context =>
                    {
                        if (context.Response.HasStarted)
                            return;

                        var localizer = context.HttpContext.RequestServices.GetRequiredService<IAppTextLocalizer>();
                        var response = CreateFrameworkErrorResponse(
                            context.HttpContext,
                            localizer,
                            StatusCodes.Status403Forbidden,
                            "ACCESS_FORBIDDEN",
                            "You do not have permission to access this endpoint.");

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions), context.HttpContext.RequestAborted);
                    }
                };
            });

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<ExternalPropertyAiOptions>(
                builder.Configuration.GetSection("ExternalPropertyAi"));
            builder.Services.Configure<AssistantAiOptions>(
                builder.Configuration.GetSection("AssistantAi"));
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
                    var localizer = context.HttpContext.RequestServices.GetRequiredService<IAppTextLocalizer>();
                    var response = CreateFrameworkErrorResponse(
                        context.HttpContext,
                        localizer,
                        StatusCodes.Status429TooManyRequests,
                        "RATE_LIMIT_EXCEEDED",
                        "Rate limit exceeded. Please try again later.");

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions), token);
                };
            });
            #endregion


            var app = builder.Build();


            #region Localization Configuration
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(LocalizationConstants.DefaultCulture),
                SupportedCultures = LocalizationConstants.SupportedCultures.ToList(),
                SupportedUICultures = LocalizationConstants.SupportedCultures.ToList()
            };

            requestLocalizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new CustomRequestCultureProvider(async context =>
                {
                    var acceptLanguageHeader = context.Request.Headers.AcceptLanguage.ToString();
                    if (!string.IsNullOrWhiteSpace(acceptLanguageHeader))
                    {
                        var requestedCulture = acceptLanguageHeader
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(value => value.Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                            .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

                        if (!string.IsNullOrWhiteSpace(requestedCulture))
                        {
                            return new ProviderCultureResult(LocalizationConstants.NormalizeCultureName(requestedCulture));
                        }
                    }

                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userCultureService = context.RequestServices.GetRequiredService<IUserCultureService>();
                    var culture = await userCultureService.ResolveUserCultureAsync(userId);
                    return new ProviderCultureResult(culture.Name);
                })
            };
            #endregion


            #region Middleware Pipeline
            app.UseRequestLocalization(requestLocalizationOptions);

            // Global Exception Handling
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors();
            //app.UseCors(builder.Configuration["AppSettings:FrontBaseUrl"]!);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            #endregion


            #region Hangfire Jobs
            var egyptTz = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            RecurringJob.AddOrUpdate<PaymentScheduleJob>(
                "payment-schedules",
                x => x.ExecuteAsync(),
                Cron.Daily(12),
                new RecurringJobOptions
                {
                    TimeZone = egyptTz
                });

            RecurringJob.AddOrUpdate<PaymentJob>(
                "payments",
                x => x.ExecuteAsync(),
                Cron.Daily(12),
                new RecurringJobOptions
                {
                    TimeZone = egyptTz
                });

            RecurringJob.AddOrUpdate<OtsUpgradeJob>(
                "ots-upgrade",
                x => x.ExecuteAsync(),
                Cron.Hourly);
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


        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static (string Code, string Message) ResolveAuthenticationChallenge(JwtBearerChallengeContext context)
        {
            if (context.AuthenticateFailure is SecurityTokenExpiredException)
            {
                return ("ACCESS_TOKEN_EXPIRED", "The access token has expired. Please sign in again.");
            }

            if (string.IsNullOrWhiteSpace(context.Request.Headers.Authorization))
            {
                return ("AUTHENTICATION_REQUIRED", "Authentication is required to access this endpoint.");
            }

            return ("ACCESS_TOKEN_INVALID", "The access token is invalid or malformed.");
        }

        private static ErrorResponse CreateFrameworkErrorResponse(
            HttpContext context,
            IAppTextLocalizer localizer,
            int statusCode,
            string code,
            string fallbackMessage)
        {
            return new ErrorResponse
            {
                Code = code,
                Message = localizer.LocalizeMessage(code, fallbackMessage),
                StatusCode = statusCode,
                Path = context.Request.Path,
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };
        }

        private static string NormalizeValidationKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key == "$")
            {
                return "body";
            }

            return key;
        }
    }
}
