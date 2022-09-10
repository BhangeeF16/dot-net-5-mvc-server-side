using AutoMapper;
using Common.ViewModels.AppSetting;
using Common.ViewModels.UserAccount;
using Domain.Entities.General;
using Domain.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserInfo, UserSignupVM>();
            CreateMap<UserSignupVM, UserInfo>();
            CreateMap<UserInfo, UserVM>();
            CreateMap<UserVM, UserInfo>();
            CreateMap<UpsertUserVM, UserInfo>();
            CreateMap<Role, RoleVM>();
            CreateMap<AppSetting, AppSettingVM>();
        }
    }
}
