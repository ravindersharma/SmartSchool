using Serilog;
using SmartSchool.Application.Common.Interfaces;

namespace SmartSchool.Infrastructure.Services.Email
{
    public class EmailSenderStub:IEmailSender
    {
        public Task SendAsync(string to, string subject, string body)
        {
            // Stub implementation - does nothing
            Log.Information("[EMAIL STUB] To: {To}, Subject: {Subject}, Body: {Body}", to, subject, body);
            return Task.CompletedTask;
        }
    }
}
