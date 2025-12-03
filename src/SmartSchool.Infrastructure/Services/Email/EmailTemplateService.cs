using SmartSchool.Application.Common.Interfaces;
using System.Reflection;

namespace SmartSchool.Infrastructure.Services.Email;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly string _basePath;

    /// <summary>
    /// If basePath not provided, defaults to an "EmailTemplates" folder next to the executing assembly.
    /// For unit tests pass a temporary folder path.
    /// </summary>
    public EmailTemplateService(string? basePath = null)
    {
        if (!string.IsNullOrWhiteSpace(basePath))
        {
            _basePath = basePath;
        }
        else
        {
            _basePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "EmailTemplates"
            );
        }
    }

    public string Render(string templateName, Dictionary<string, string> placeholders)
    {
        string path = Path.Combine(_basePath, $"{templateName}.html");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Template '{templateName}' not found at '{path}'");

        string content = File.ReadAllText(path);

        foreach (var (key, value) in placeholders)
        {
            content = content.Replace($"{{{{{key}}}}}", value);
        }

        return content;
    }
}
