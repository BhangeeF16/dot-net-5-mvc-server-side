using Common.ViewModels.UserAccount;

using System.Security.Claims;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Common.Utilities
{
    public static class GeneralUtility
    {
        public static UserVM GetCurrentUserInfo(ClaimsPrincipal userClaims)
        {
            var userInfo = userClaims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            if (userInfo == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<UserVM>(userInfo.Value);
        }

        public static string RemoveSpecialCharacters (string str)
        {
            StringBuilder sb = new StringBuilder();

            if (str != null)
            {
                foreach (char c in str)
                {
                    if ((c >= '0' && c <= '9'))
                    {
                        sb.Append(c);
                    }
                }
             
            }
            return sb.ToString();


        }
    }
}