#region Imports

using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Utilities;
using Common.ViewModels.UserAccount;
#endregion

namespace Domain.IServices
{
    public interface IUsersService
    {
        Task<IEnumerable<UserVM>> GetAll();
        Task<UserVM> GetByID(int id);
        Task<UserVM> GetUserByID(string id);
        Task<IEnumerable<string>> GetUserRole(UserVM user);
        Task<ResponseMessage> UpsertUser(UpsertUserVM user);
        Task<UserVM> Create(UserSignupVM user);
        Task<UserVM> Delete(int id);
        Task<ResponseMessage> UpdateAccountStatus(bool status, string id);
        List<RoleVM> GetAllRoles(bool activeRoles);
        ResponseMessage UpdateRoleStatus(RoleVM roleVM);
        ResponseMessage UpsertRole(RoleVM roleVM);


    }
}