using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Data.Services
{
    public class EmailRepository: IEmailRepository
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IConfiguration"></param>
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendActivationEmail(string from, string to, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmail(string From, string To, string Subject, string Body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = @"<div style='background-color:#f7f7f7; padding:70px; font-family:Arial, Helvetica; font-size:12px;'>
                    <div style='box-shadow: rgba(0, 0, 0, 0.1) 0px 10px 50px; background-color:#ffffff; padding:70px;'>
                        <p>You've specified that you didn't have much experience in trading. To help you have a good start with us, we have prepared a series of 10 short letters.</p>
                        <p><strong>Get Started By Activating your Account</strong><br>Click the link bellow to activate your account.</p>
                        <p>" + Body+@"</p>
                    </div> 
               </div>";
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:SmtpHost"], Int32.Parse(_configuration["Email:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:SmtpUser"], _configuration["Email:SmtpPass"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPasswordResetLink(string From, string To, string Subject, string Body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = @"<div style='background-color:#f7f7f7; padding:70px; font-family:Arial, Helvetica; font-size:12px;'>
                    <div style='box-shadow: rgba(0, 0, 0, 0.1) 0px 10px 50px; background-color:#ffffff; padding:70px;'>
                        <p>You've specified that you didn't have much experience in trading. To help you have a good start with us, we have prepared a series of 10 short letters.</p>
                        <p><strong>Click on the link bellow to reset your password.</strong><br>Click the link bellow to reset your password.</p>
                        <p>" + Body + @"</p>
                    </div> 
               </div>";
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:SmtpHost"], Int32.Parse(_configuration["Email:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:SmtpUser"], _configuration["Email:SmtpPass"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
