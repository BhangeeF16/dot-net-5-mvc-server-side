using Common.ViewModels.UserAccount;
using Domain.IServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private UserVM user;
        public string BaseUrl { get; }
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            contextAccessor = httpContextAccessor;
            BaseUrl = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host.Value.ToString()}{httpContextAccessor.HttpContext?.Request.PathBase.Value.ToString()}";
            Fullname = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

            var userInfo = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.UserData);
            if (userInfo != null) user = JsonConvert.DeserializeObject<UserVM>(userInfo);
        }

        public string Fullname { get; }

        public UserVM User
        {
            get
            {
                if (user != null) return user;
                if (!contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException();
                var userInfo = contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.UserData);
                if (userInfo != null) user = JsonConvert.DeserializeObject<UserVM>(userInfo);
                return user ?? new UserVM();
            }
        }

        public string UserId => User.Id;
    }
}
