using Serilog;
using SmartSchool.Application;
using SmartSchool.Infrastructure;
using SmartSchool.Worker;
using SmartSchool.Application.Common.Interfaces;
using SmartSchool.Infrastructure.Services.Email;

internal class Program
{
    private static void Main(string[] args)
    {
        // 1) Create Builder (NET 10 Worker)
        var builder = Host.CreateApplicationBuilder(args);

        // 2) Configure Serilog correctly for .NET 10
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)   // ✔ Works if "Serilog" section exists
            .WriteTo.Console()
            .CreateLogger();

        // register with logging subsystem
        builder.Services.AddLogging(lb =>
        {
            lb.ClearProviders();
            lb.AddSerilog(logger, dispose: true);
        });

        // 3) Register App + Infra
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // 4) Email Template Service
        builder.Services.AddSingleton<IEmailTemplateService>(sp => new EmailTemplateService());

        // 5) Email Queue
        builder.Services.AddSingleton<IEmailQueue, EmailQueue>();

        // 6) Worker
        builder.Services.AddHostedService<EmailQueueWorker>();

        // 7) Run
        var host = builder.Build();
        host.Run();
    }
}