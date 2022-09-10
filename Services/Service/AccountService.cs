using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Helper;
using Common.Utilities;
using Common.ViewModels.ResetPassword;
using Common.ViewModels.UserAccount;
using Domain;
using Domain.Entities.IdentityModule;
using Domain.IRepositories;
using Domain.IServices;

using Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<UserInfo> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public AccountService(IUsersService usersService1, IMapper mapper, IUnitOfWork unitOfWork, UserManager<UserInfo> userManager, RoleManager<Role> roleManager, ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _usersService = usersService1;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _currentUserService = currentUser;
        }
        public async Task<UserVM> Authenticate(string username, string password)
        {
            var userInfo = await _unitOfWork.UserRepository.GetFirstOrDefault(x => x.UserName.ToLower() == username.ToLower() && x.IsActive);
            if (userInfo != null)
            {
                var activeRoles = _usersService.GetAllRoles(activeRoles: true).Select(x => x.Name).ToList();
                var userRoles = await _userManager.GetRolesAsync(userInfo);
                userRoles = userRoles.Where(c => activeRoles.Any(x => x == c)).ToList();
                if (userInfo.PasswordHash.VerifyWithBCrypt(password))
                {
                    return new UserVM
                    {
                        Id = userInfo.Id,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Email = userInfo.UserName,
                        Roles = userRoles.ToList(),
                    };
                }
            }
            return null;
        }
        public async Task<ResponseMessage> ChangePassword(ChangePasswordVM changePasswordVm)
        {
            ResponseMessage responseMessage = new();
            try
            {
                var id = _currentUserService.UserId;
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    if (user.PasswordHash.VerifyWithBCrypt(changePasswordVm.OldPassword))
                    {

                        user.PasswordHash = changePasswordVm.NewPassword.WithBCrypt();
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Complete();
                        responseMessage.Success = true;
                    }
                    else
                    {
                        responseMessage.Success = false;
                        responseMessage.Error = "Old password does not match";
                    }
                }
            }
            catch (Exception)
            {
                responseMessage.Success = false;
                responseMessage.Error = "Something went wrong";
            }

            return responseMessage;
        }


        public async Task<ResponseMessage> ResetPassword(ResetPasswordVM resetPasswordModel)
        {
            ResponseMessage responseMessage = new();
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
                if (user != null && user.IsActive)
                {
                    user.PasswordHash = resetPasswordModel.Password.WithBCrypt();
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Complete();
                    responseMessage.Success = true;
                }
                else
                {
                    responseMessage.Success = false;
                    responseMessage.Error = "Email not found";
                }
            }
            catch (Exception ex)
            {
                responseMessage.Success = false;
                responseMessage.Error = "Something went wrong";
            }
            return responseMessage;
        }

    }
}
