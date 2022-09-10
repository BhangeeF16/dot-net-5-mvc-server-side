using Common.Utilities;
using Domain.Entities.IdentityModule;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IEmailService
    {
        void SendMail(string toEmail, string subject, string body, bool isBodyHtml = true);
        Task<bool> UserRegisterEmail(UserInfo user);
        Task<ResponseMessage> ForgotPassword(string Email);
    }
}
