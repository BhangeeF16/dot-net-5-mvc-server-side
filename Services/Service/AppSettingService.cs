using AutoMapper;
using Common.ViewModels.AppSetting;
using Domain.Entities.General;
using Domain.IRepositories;
using Domain.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _iConfig;

        public AppSettingService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration iConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _iConfig = iConfig;
        }
        public List<AppSettingVM> GetAppSetting()
        {
            List<AppSettingVM> appSettingVMs = null;
            try
            {
                var appsettings = _unitOfWork.AppSettingRepository.GetAll().Result.ToList();
                appSettingVMs = _mapper.Map<List<AppSettingVM>>(appsettings);
            }
            catch { }
            return appSettingVMs;
        }

        public AppSettingVM GetAppSettingByID(int ID)
        {
            AppSettingVM result = new AppSettingVM();
            try
            {
                var setting = _unitOfWork.AppSettingRepository.GetFirstOrDefault(c => c.ID == ID).Result;
                return _mapper.Map<AppSettingVM>(setting);
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public AppSettingVM GetAppSettingByName(string name)
        {
            AppSettingVM result = new AppSettingVM();
            try
            {
                var setting = _unitOfWork.AppSettingRepository.GetFirstOrDefault(c => c.Name.ToLower() == name.ToLower()).Result;
                return _mapper.Map<AppSettingVM>(setting);
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public bool UpdateAppSetting(List<AppSettingVM> appSettings)
        {
            try
            {
                foreach (var setting in appSettings)
                {
                    var settingObj = _unitOfWork.AppSettingRepository.GetFirstOrDefault(x => x.ID == setting.ID).Result;
                    if (settingObj != null)
                    {
                        settingObj.Value = setting.Value;
                        _unitOfWork.AppSettingRepository.Update(settingObj);
                    }
                }
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddAppSetting(List<AppSetting> appSettings)
        {
            try
            {
                foreach (var setting in appSettings)
                {
                    _unitOfWork.AppSettingRepository.Add(setting);
                }
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
