using Common.ViewModels.AppSetting;
using Domain.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IAppSettingService
    {
        List<AppSettingVM> GetAppSetting();
        AppSettingVM GetAppSettingByID(int ID);
        AppSettingVM GetAppSettingByName(string name);
        bool UpdateAppSetting(List<AppSettingVM> appSettings);
        bool AddAppSetting(List<AppSetting> appSettings);
    }
}
