using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Data.ViewModels
{
    public class AccountActivationVM
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
