using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Data.Services
{
    public interface IEmailRepository
    {
        public Task SendActivationEmail(string from, string to, string subject, string body);
        public Task SendPasswordResetLink(string from, string to, string subject, string body);
    }
}
