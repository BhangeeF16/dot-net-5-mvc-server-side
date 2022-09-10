using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
   public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
    }
}
