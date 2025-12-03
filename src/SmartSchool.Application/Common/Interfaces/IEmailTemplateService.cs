namespace SmartSchool.Application.Common.Interfaces
{
    public interface IEmailTemplateService
    {
        string Render(string templateName, Dictionary<string, string> placeholders);
    }
}
