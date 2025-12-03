using SmartSchool.Application.Common.Interfaces;
using System.Reflection;

namespace SmartSchool.Infrastructure.Services.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public readonly string _basePath;

        public EmailTemplateService()
        {
            _basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "EmailTemplates");
        }
        public string Render(string templateName, Dictionary<string, string> placeholders)
        {
            string path = Path.Combine(_basePath, $"{templateName}.html");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Template '{templateName}' not found at '{path}'");

            string content = File.ReadAllText(path);

            foreach (var placeholder in placeholders)
            {
                content = content.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return content;
        }
    }
}
