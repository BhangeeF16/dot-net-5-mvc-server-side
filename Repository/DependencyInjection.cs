#region Imports

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Repository;
using Repository.Seed;
using Repository.Persistence;

#endregion

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DataSeeder>();
            services.AddDbContext<AppDbContext>(opt => opt
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}