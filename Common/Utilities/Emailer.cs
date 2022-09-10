using Common.ViewModels.AppSetting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class Emailer
    {
        public static Task<bool> SendEmail(string subject, string body, List<AppSettingVM> appSettings, string to)
        {
            var tcs = new TaskCompletionSource<bool>();
            var mail = new MailMessage
            {
                From = new MailAddress(appSettings.Where(x => x.Name == SystemSettingsVariables.FromEmail).FirstOrDefault().Value),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(to));
            var client = new SmtpClient
            {
                Host = appSettings.Where(x => x.Name == SystemSettingsVariables.SmtpClient).FirstOrDefault().Value,
                Port = Convert.ToInt32(appSettings.Where(x => x.Name == SystemSettingsVariables.SmtpPort).FirstOrDefault().Value),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(appSettings.Where(x => x.Name == SystemSettingsVariables.SmtpUser).FirstOrDefault().Value, appSettings.Where(x => x.Name == SystemSettingsVariables.SmtpPassword).FirstOrDefault().Value)
            };
            try
            {
                client.Send(mail);
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                return tcs.Task;
            }
            return tcs.Task;
        }

    }
}
