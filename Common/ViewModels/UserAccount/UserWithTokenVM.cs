using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ViewModels.UserAccount
{
    public class UserWithTokenVM
    {
        public string AccessToken { get; set; }
        public UserVM User { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime RequestedServerUtcNow { get; set; }
    }
}
