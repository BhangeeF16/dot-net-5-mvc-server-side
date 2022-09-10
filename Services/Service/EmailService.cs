using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Common.Utilities;
using Common.ViewModels.AppSetting;
using Domain.Entities.IdentityModule;
using Domain.IServices;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppSettingService _appSettingService;

        public EmailService(UserManager<UserInfo> userManager, ICurrentUserService currentUser, IAppSettingService appSettingService)
        {
            _userManager = userManager;
            _currentUserService = currentUser;
            _appSettingService = appSettingService;
        }
        public void SendMail(string toEmail, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("", "R3Investment Group");
                    mail.To.Add(new MailAddress(toEmail));
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isBodyHtml;
                    using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("", "");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> UserRegisterEmail(UserInfo user)
        {
            try
            {
                if (user != null)
                {
                    var url = _currentUserService.BaseUrl;
                    string body = "";
                    var appSettings = _appSettingService.GetAppSetting();
                    var name = user.FirstName + " " + user.LastName;
                    var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword");
                    string callbackUrl = $"{_currentUserService.BaseUrl}/Account/ResetPassword?token={HttpUtility.UrlEncode(token)}";
                    string path = url + "/home/resetpasswordtemplate";

                    using (var webClient = new WebClient())
                    {
                        body = webClient.DownloadString(path);
                    }
                    body = body.Replace("{User}", name);
                    body = body.Replace("{ResetLink}", callbackUrl);
                    await Emailer.SendEmail("Welcome | ProjectionHub", body, appSettings, user.Email);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ResponseMessage> ForgotPassword(string Email)
        {
            ResponseMessage responseMessage = new();
            try
            {
                if (Email != null)
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    if (user != null)
                    {
                        var url = _currentUserService.BaseUrl;
                        string body = "";
                        var appSettings = _appSettingService.GetAppSetting();
                        var name = user.FirstName + " " + user.LastName;
                        var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword");
                        string callbackUrl = $"{_currentUserService.BaseUrl}/Account/ResetPassword?token={HttpUtility.UrlEncode(token)}";
                        string path = url + "/home/forgotpasswordtemplate";

                        using (var webClient = new WebClient())
                        {
                            body = webClient.DownloadString(path);
                        }
                        body = body.Replace("{User}", name);
                        body = body.Replace("{ResetLink}", callbackUrl);
                        await Emailer.SendEmail("Reset Password", body, appSettings, user.Email);
                        responseMessage.Success = true;
                    }
                    else
                    {
                        responseMessage.Success = false;
                        responseMessage.Error = "Email not found";
                    }

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
