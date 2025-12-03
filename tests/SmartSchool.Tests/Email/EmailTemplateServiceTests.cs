using FluentAssertions;
using SmartSchool.Infrastructure.Services.Email;

namespace SmartSchool.Tests.Email;

public class EmailTemplateServiceTests : IDisposable
{
    private readonly string _tempDir;

    public EmailTemplateServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"email_templates_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);

        // create sample templates
        File.WriteAllText(Path.Combine(_tempDir, "welcome.html"), "<p>Welcome {{UserName}}</p>");
        File.WriteAllText(Path.Combine(_tempDir, "otp.html"), "<p>OTP: {{Otp}}</p>");
    }

    [Fact]
    public void Render_ShouldReplacePlaceholders_ForExistingTemplate()
    {
        var service = new EmailTemplateService(_tempDir);

        var result = service.Render("welcome", new Dictionary<string, string> { { "UserName", "Ravi" } });

        result.Should().Contain("Welcome Ravi");
    }

    [Fact]
    public void Render_ShouldThrow_WhenTemplateMissing()
    {
        var service = new EmailTemplateService(_tempDir);

        Action act = () => service.Render("non-existent", new Dictionary<string, string>());

        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void Render_ShouldReplaceMultiplePlaceholders()
    {
        var service = new EmailTemplateService(_tempDir);

        var result = service.Render("otp", new Dictionary<string, string> { { "Otp", "12345" } });

        result.Should().Contain("OTP: 12345");
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tempDir, true);
        }
        catch { /* ignore cleanup errors */ }
    }
}
