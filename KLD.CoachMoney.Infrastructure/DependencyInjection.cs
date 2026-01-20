using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Company.Template.Application.Interfaces;
using Company.Template.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Template.Infrastructure
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

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
