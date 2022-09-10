using Common.Utilities;
using Common.ViewModels.AppSetting;
using Common.ViewModels.UserAccount;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppSettingService _appSettingService;
        public UsersController(IUsersService usersService, ICurrentUserService currentUserService, IAppSettingService appSettingService)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
            _appSettingService = appSettingService;
        }
        [Authorize]
        public ActionResult SystemSetting()
        {
            return View(_appSettingService.GetAppSetting());
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateAppSetting(List<AppSettingVM> appSettings)
        {
            var result = _appSettingService.UpdateAppSetting(appSettings);
            return Json(result);
        }
        public IActionResult ManageUsers()
        {
            var userinfo = GeneralUtility.GetCurrentUserInfo(User);
            var role = userinfo.Roles.FirstOrDefault();
            ViewBag.Roles = _usersService.GetAllRoles(activeRoles: true);
            var userList = _usersService.GetAll().Result;

            if (userList != null && userList.Any())
            {
                foreach (var user in userList)
                {
                    user.Roles = _usersService.GetUserRole(user).Result.ToList();
                }
                if (role != "Agent")
                {
                    userList = userList;
                }
                else
                {
                    userList = userList.Where(x => x.Roles[0].Contains("Driver"));
                }
            }
            return View(userList);
        }

        public IActionResult UserRoles()
        {
            return View(_usersService.GetAllRoles(activeRoles: false));
        }

        [HttpPost]
        public IActionResult UpdateRoleStatus(RoleVM roleVM)
        {
            return Json(_usersService.UpdateRoleStatus(roleVM));
        }
        [HttpPost]
        public IActionResult UpsertRole(RoleVM roleVM)
        {
            return Json(_usersService.UpsertRole(roleVM));
        }

        [HttpPost]
        public async Task<IActionResult> GetUserInfo(string userId)
        {
            try
            {
                var userInformation = await _usersService.GetUserByID(userId);
                if (userInformation != null)
                {
                    userInformation.Roles = _usersService.GetUserRole(userInformation).Result.ToList();
                }
                return Ok(userInformation);
            }
            catch (Exception e)
            {
                return Ok(new());
            }

        }

        public async Task<IActionResult> UpsertUser(UpsertUserVM addUser)
        {
            try
            {
                if (addUser.UserId != "0")
                {
                    addUser.ModifiedBy = _currentUserService.User?.Id;
                    addUser.ModifiedDate = DateTime.Now;

                }
                else
                {
                    addUser.CreatedBy = _currentUserService.User?.Id;
                    addUser.CreatedDate = DateTime.Now;
                }
                return Json(await _usersService.UpsertUser(addUser));
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(bool status, string id)
        {
            try
            {
                return Json(await _usersService.UpdateAccountStatus(status, id));
            }
            catch (Exception e)
            {
                return Json(new());
            }
        }

    }
}
