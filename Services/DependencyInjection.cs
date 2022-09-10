#region Imports

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Services.Service;
using Domain.IServices;


#endregion

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepository(configuration);
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAppSettingService, AppSettingService>();
            return services;
        }
    }
}