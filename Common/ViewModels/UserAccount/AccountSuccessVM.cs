using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ViewModels.UserAccount
{
    public class AccountSuccessVM
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public Exception ErrorMessage { get; set; }
    }
}
