using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartSchool.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SmartSchool.Infrastructure.Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"] ?? "25");
            var user = _config["Smtp:User"];
            var pass = _config["Smtp:Password"];

            if (user == null) throw new Exception("SMTP User not configured");


            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            var mail = new MailMessage(user, to, subject, body)
            {
                IsBodyHtml = true
            };


            await client.SendMailAsync(mail);

        }
    }
}
