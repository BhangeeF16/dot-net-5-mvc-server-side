using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Utilities;
using Common.ViewModels.ResetPassword;
using Common.ViewModels.UserAccount;

using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.IServices
{
    public interface IAccountService
    {
        Task<UserVM> Authenticate(string username, string password);
        Task<ResponseMessage> ChangePassword(ChangePasswordVM changePasswordVm);
        Task<ResponseMessage> ResetPassword(ResetPasswordVM resetPasswordModel);

    }
}
