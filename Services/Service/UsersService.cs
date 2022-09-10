#region Imports

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Extensions;
using Common.Helper;
using Common.ViewModels.UserAccount;
using Infrastructure.Persistence;
using Domain.IRepositories;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Common.Utilities;
using Common.ViewModels.AppSetting;
using Domain.IServices;
using Domain.Entities.IdentityModule;

#endregion

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _iConfig;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<UserInfo> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;

        public UsersService(IUnitOfWork unitOfWork, ICurrentUserService currentUser, IMapper mapper, IConfiguration iConfig, RoleManager<Role> roleManager, UserManager<UserInfo> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _iConfig = iConfig;
            _roleManager = roleManager;
            _userManager = userManager;
            _currentUserService = currentUser;
            _emailService = emailService;
        }

        public async Task<IEnumerable<UserVM>> GetAll()
        {
            var lstUsers = await _unitOfWork.UserRepository.GetAll();
            return lstUsers != null && lstUsers?.Count() > 0 ? _mapper.Map<IEnumerable<UserVM>>(lstUsers) : null;
        }

        public async Task<IEnumerable<string>> GetUserRole(UserVM user)
        {
            var userInfo = _mapper.Map<UserInfo>(user);
            var userRole = await _userManager.GetRolesAsync(userInfo);
            return userRole;

        }

        public async Task<UserVM> GetByID(int id)
        {
            var user = await _unitOfWork.UserRepository.Get(id);
            return user != null ? _mapper.Map<UserVM>(user) : null;
        }

        public async Task<UserVM> GetUserByID(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByString(id);
            return user != null ? _mapper.Map<UserVM>(user) : null;
        }

        //public async Task<UserVM> Update(AddUserVM user)
        //{
        //    UserInfo entity = null;
        //    if (user != null)
        //    {

        //        var userInDB =  _unitOfWork.UserRepository.GetByString(user.UserId).GetAwaiter().GetResult();

        //        if (userInDB != null)
        //        {
        //            // edit
        //            userInDB.FirstName = user.FirstName;
        //            userInDB.LastName = user.LastName;
        //            userInDB.Active = user.Active;
        //            userInDB.Email = user.Email;
        //            userInDB.PhoneNumber = user.PhoneNumber;
        //            _unitOfWork.UserRepository.Update(userInDB);

        //        }
        //        entity = userInDB;

        //    }
        //    var r = _unitOfWork.Complete();
        //    return r > 0 ? _mapper.Map<UserVM>(entity) : null;
        //}

        public async Task<UserVM> Delete(int id)
        {
            UserInfo entity = null;
            if (id > 0)
            {
                var entityToDelete = await _unitOfWork.UserRepository.Get(id);
                if (entityToDelete != null)
                {
                    _unitOfWork.UserRepository.Update(entityToDelete);
                    entity = entityToDelete;
                }
            }

            _unitOfWork.Complete();
            return _mapper.Map<UserVM>(entity);
        }

        public async Task<ResponseMessage> UpsertUser(UpsertUserVM user)
        {
            ResponseMessage responseMessage = new();
            try
            {
                if (user != null)
                {
                    if (user.UserId != "0") // Update
                    {
                        var userInDB = _unitOfWork.UserRepository.GetWhere(c => c.Id == user.UserId).Result.FirstOrDefault();

                        if (userInDB != null)
                        {
                            if (user.Email != userInDB.Email && _userManager.Users.Any(c => c.Email == user.Email))
                            {
                                responseMessage.Success = false;
                                responseMessage.Error = "Email already exists";
                            }
                            else
                            {
                                bool previousStatus = userInDB.IsActive;
                                // edit
                                userInDB.FirstName = user.FirstName;
                                userInDB.LastName = user.LastName;
                                userInDB.IsActive = user.Active;
                                userInDB.Email = user.Email;
                                userInDB.Gender = user.Gender;
                                userInDB.Address = user.Address;
                                userInDB.EmergencyContactName = user.EmergencyContactName;
                                userInDB.EmergencyContactNo = GeneralUtility.RemoveSpecialCharacters(user.EmergencyContactNo);
                                userInDB.DriverLicense = user.DriverLicense;
                                userInDB.PrimaryPhoneNumber = GeneralUtility.RemoveSpecialCharacters(user.PrimaryPhoneNumber);
                                _unitOfWork.Complete();
                                // role management
                                var userRolesInDB = _userManager.GetRolesAsync(userInDB).Result;
                                var userRolesToDelete = userRolesInDB.Where(c => c != user.UserRole).ToList();
                                var userRolesToAdd = userRolesInDB.Where(c => c == user.UserRole).ToList();
                                if (userRolesToDelete?.Count > 0)
                                {
                                    _userManager.RemoveFromRolesAsync(userInDB, userRolesToDelete).GetAwaiter().GetResult();
                                }
                                if (userRolesToAdd?.Count == 0)
                                {
                                    _userManager.AddToRoleAsync(userInDB, user.UserRole).GetAwaiter().GetResult();
                                }
                                if (previousStatus != user.Active && user.Active)
                                {
                                    await _emailService.UserRegisterEmail(userInDB);
                                }

                                responseMessage.Success = true;

                            }
                        }
                        else
                        {
                            responseMessage.Success = false;
                            responseMessage.Error = "User not found";
                        }
                    }
                    else // Add
                    {
                        if (_userManager.Users.Any(c => c.Email == user.Email))
                        {
                            responseMessage.Success = false;
                            responseMessage.Error = "Email already exists";
                        }
                        else
                        {
                            user.Id = Guid.NewGuid().ToString();
                            user.UserName = user.Email;
                            user.DriverLicense = user.DriverLicense;
                            user.EmergencyContactName = user.EmergencyContactName;
                            user.EmergencyContactNo = GeneralUtility.RemoveSpecialCharacters(user.EmergencyContactNo);
                            user.Address = user.Address;
                            user.PasswordHash = GetRandomPassword();
                            user.PrimaryPhoneNumber = GeneralUtility.RemoveSpecialCharacters(user.PrimaryPhoneNumber);
                            user.SecondaryPhoneNumber = GeneralUtility.RemoveSpecialCharacters(user.SecondaryPhoneNumber);
                            var entity = _mapper.Map<UserInfo>(user);

                            await _userManager.CreateAsync(entity);
                            _userManager.AddToRoleAsync(entity, user.UserRole).GetAwaiter().GetResult();
                            // Send Email
                            await _emailService.UserRegisterEmail(entity);
                            responseMessage.Success = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                responseMessage.Success = false;
                responseMessage.Error = "Something went wrong";
            }
            return responseMessage;
        }



        public async Task<UserVM> Create(UserSignupVM user)
        {
            UserInfo entity = null;
            if (user != null)
            {
                //var userToAdd = new UserInfo(user.FirstName, user.LastName, user.Password, user.Email, user.Phone, user.Admin);
                //if (userToAdd.Password.IsNotNullOrWhiteSpace())
                //    userToAdd.Password = userToAdd.Password.WithBCrypt();
                //else
                //    userToAdd.Password = "";
                //await _unitOfWork.UserRepository.Add(userToAdd);
                //entity = userToAdd;
            }

            _unitOfWork.Complete();
            return _mapper.Map<UserVM>(entity);
        }

        public async Task<ResponseMessage> UpdateAccountStatus(bool status, string id)
        {
            var responseMessage = new ResponseMessage();
            var userInDB = await _unitOfWork.UserRepository.GetByString(id);
            if (userInDB != null)
            {
                var previousStatus = userInDB.IsActive;
                userInDB.IsActive = status;
                _unitOfWork.UserRepository.Update(userInDB);
                _unitOfWork.Complete();
                if (previousStatus != status && status)
                {
                    await _emailService.UserRegisterEmail(userInDB);
                }
                responseMessage.Success = true;
            }
            else
            {
                responseMessage.Success = false;
                responseMessage.Error = "User not found";
            }
            return responseMessage;
        }

        public List<RoleVM> GetAllRoles(bool activeRoles = false)
        {
            List<RoleVM> roles = null;
            if (activeRoles)
            {
                roles = _roleManager.Roles.Where(c => c.IsActive).Select(c => new RoleVM { Id = c.Id, Name = c.Name, IsActive = c.IsActive }).ToList();
            }
            else
            {
                roles = _roleManager.Roles.Select(c => new RoleVM { Id = c.Id, Name = c.Name, IsActive = c.IsActive }).ToList();
            }
            return roles;
        }
        public ResponseMessage UpdateRoleStatus(RoleVM roleVM)
        {
            ResponseMessage result = new ResponseMessage();
            try
            {
                var role = _roleManager.Roles.FirstOrDefault(c => c.Id == roleVM.Id);
                if (role != null)
                {
                    role.IsActive = roleVM.IsActive;
                    var isUpdated = _roleManager.UpdateAsync(role).Result;
                    result.Success = isUpdated.Succeeded;
                }
                else
                {
                    result.Success = false;
                    result.Error = "Role not found";
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = "Something went wrong!! Please Contact to administrator";
            }
            return result;
        }
        public ResponseMessage UpsertRole(RoleVM roleVM)
        {
            var result = new ResponseMessage();
            try
            {
                if (roleVM.Id == "0")
                {
                    var role = _roleManager.Roles.FirstOrDefault(c => c.NormalizedName.Trim() == roleVM.Name.Trim().ToUpper());
                    if (role == null)
                    {
                        role = new Role
                        {
                            Name = roleVM.Name.Trim(),
                            NormalizedName = roleVM.Name.Trim().ToUpper(),
                            IsActive = roleVM.IsActive
                        };
                        var isCreated = _roleManager.CreateAsync(role).Result;
                        result.Success = isCreated.Succeeded;
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = "Role already exists";
                    }
                }
                else
                {
                    var role = _roleManager.Roles.FirstOrDefault(c => c.Id == roleVM.Id);
                    if (role != null)
                    {
                        var roleName = _roleManager.Roles.FirstOrDefault(c => c.NormalizedName.Trim() == roleVM.Name.Trim().ToUpper());
                        if (roleName == null)
                        {
                            role.IsActive = roleVM.IsActive;
                            role.Name = roleVM.Name.Trim();
                            role.NormalizedName = roleVM.Name.Trim().ToUpper();
                            var isUpdated = _roleManager.UpdateAsync(role).Result;
                            result.Success = isUpdated.Succeeded;
                        }
                        else
                        {
                            result.Success = false;
                            result.Error = "Role already exists";
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = "Role not found";
                    }
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = "Something went wrong!! Please Contact to administrator";
            }
            return result;
        }

        private static string GetRandomPassword()
        {
            return Guid.NewGuid().ToString().Replace("-", "")[..8];
        }
    }
}
