using Common.Helper;
using Common.ViewModels.AppSetting;
using Domain.Entities.General;
using Domain.Entities.IdentityModule;
using Domain.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Seed
{
    public class DataSeeder
    {
        private UserManager<UserInfo> _userManager;
        private RoleManager<Role> _roleManager;
        private IAppSettingService _appSettingService;
        public DataSeeder(UserManager<UserInfo> userManager, RoleManager<Role> roleManager, IAppSettingService appSettingService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettingService = appSettingService;
        }
        public void SeedUserAndRoles()
        {
            string[] roles = new string[] { "ApplicationAdmin", "Trainee", "Student" };
            foreach (string role in roles)
            {
                if (!_roleManager.Roles.Any(r => r.Name == role))
                {
                    var roleResult = _roleManager.CreateAsync(new Role
                    {
                        IsActive = true,
                        Name = role,
                        NormalizedName = role.ToUpper()
                    }).Result;
                }
            }

            var user = new UserInfo
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@codered.com",
                NormalizedEmail = "admin@codered.com".ToUpper(),
                UserName = "admin@codered.com",
                NormalizedUserName = "admin@codered.com".ToUpper(),
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                Gender = "Male",
            };


            if (!_userManager.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<UserInfo>();
                user.PasswordHash = "admin123".WithBCrypt();
                var userResult = _userManager.CreateAsync(user).Result;
                var userRoleResult = _userManager.AddToRolesAsync(user, roles).Result;
            }
            if (_appSettingService.GetAppSetting().Count == 0)
            {
                var appSettings = new List<AppSetting>();
                appSettings.Add(new AppSetting
                {
                    ID = 0,
                    Name = "FromMail",
                    Value = "test.Dev1.email@gmail.com",
                    Label = "From Mail",
                    Description = "Notification From Mail Address",
                });
                appSettings.Add(new AppSetting
                {
                    ID = 0,
                    Name = "SmtpClient",
                    Value = "smtp.gmail.com",
                    Label = "SMTP Client",
                    Description = "SMTP Client URL",
                });
                appSettings.Add(new AppSetting
                {
                    ID = 0,
                    Name = "SmtpPort",
                    Value = "587",
                    Label = "SMTP Port",
                    Description = "SMTP Port",
                });
                appSettings.Add(new AppSetting
                {
                    ID = 0,
                    Name = "SmtpUser",
                    Value = "test.Dev1.email@gmail.com",
                    Label = "SMTP User Name",
                    Description = "SMTP User Name",
                });
                appSettings.Add(new AppSetting
                {
                    ID = 0,
                    Name = "SmtpPassword",
                    Value = "Abc!2345",
                    Label = "SMTP Password",
                    Description = "SMTP Password",
                });
                _appSettingService.AddAppSetting(appSettings);
            }


        }
    }
}
