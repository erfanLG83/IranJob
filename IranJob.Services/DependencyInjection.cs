using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IranJob.Domain.Auth;
using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.Services.Contract;
using IranJob.Services.Exeptions;
using IranJob.Services.Features;
using IranJob.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IranJob.Services
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Services Such a IEmailSender
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<AppIdentityErrorDescriber>();
            services.AddScoped<IAppUserManager,AppUserManager>();
            services.AddIdentity<AppUser, IdentityRole>(
                op =>
                {
                    op.User.RequireUniqueEmail = true;
                    op.Password.RequireDigit = false;
                    op.Password.RequireLowercase = false;
                    op.Password.RequireUppercase = false;
                    op.Password.RequiredLength = 8;
                    op.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<IranJobDbContext>()
                .AddUserManager<AppUserManager>()
                //.AddRoleManager<>
                .AddErrorDescriber<AppIdentityErrorDescriber>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes("1234567890asdfgh");
                var encryptionkey = Encoding.UTF8.GetBytes("qwsadfrewtyh4532");
                var validationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = "IranJob",

                    ValidateIssuer = true, //default : false
                    ValidIssuer = "IranJob",

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        if (context.Exception != null)
                            throw new ApiUnAutorizationExcpetion(
                                ApiResultStatusCode.UnAuthorized,
                                "Un Autorization",
                                System.Net.HttpStatusCode.Unauthorized,
                                context.Exception,
                                null
                            );
                        await Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices
                            .GetRequiredService<IAppUserManager>();
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("this token havent claims");
                        var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)
                            ?.Value;
                        if (string.IsNullOrEmpty(securityStamp))
                            context.Fail("this token has no security stamp");
                        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        var user = await userManager.FindByIdAsync(userId);
                        if (user.SecurityStamp != securityStamp)
                            context.Fail("token security stamp is not valid");
                    },
                    OnChallenge = context =>
                    {
                        if (context.AuthenticateFailure != null)
                            throw new ApiUnAutorizationExcpetion(ApiResultStatusCode.UnAuthorized,
                                context.AuthenticateFailure.Message, context.AuthenticateFailure, null);
                        throw new ApiUnAutorizationExcpetion(ApiResultStatusCode.UnAuthorized, "you are unautorized");
                    }

                };
                options.TokenValidationParameters = validationParameters;
            });
            return services;
        }
        
    }
}
