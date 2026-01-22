using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.AiServices;
using KLD.CoachMoney.Application.Abstractions.AIServices;
using KLD.CoachMoney.Application.Abstractions.Auth;
using KLD.CoachMoney.Infrastructure.AI;
using KLD.CoachMoney.Infrastructure.Auth;
using KLD.CoachMoney.Infrastructure.Financial;
using KLD.CoachMoney.Infrastructure.Identity;
using KLD.CoachMoney.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    b => b.MigrationsAssembly($"{Assembly.GetAssembly(typeof(ApplicationDbContext))!.FullName}")
                );
            });

            var aiOptions = configuration
                .GetSection("FinancialAi")
                .Get<FinancialAiOptions>()
                ?? throw new InvalidOperationException("FinancialAi config missing");

            services.AddSingleton(aiOptions);

            services.AddHttpClient<IFinancialAiService, FinancialAiService>();

            var jwtOptions = configuration
                .GetSection("Jwt")
                .Get<JwtOptions>()
                ?? throw new InvalidOperationException("Jwt config missing");

            services.AddSingleton(jwtOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtOptions.Key)),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IFinancialAiService, FinancialAiService>();
            services.AddScoped<IFinancialSnapshotProvider, FinancialSnapshotProvider>();

        }
    }
}
