using SmartSchool.Application.Common.Interfaces;
using System.Net.Sockets;

namespace SmartSchool.Worker;

public class EmailQueueWorker : BackgroundService
{
    private readonly IEmailQueue _queue;
    private readonly IEmailSender _sender;
    private readonly ILogger<EmailQueueWorker> _logger;
    private readonly int _maxRetries;
    private readonly TimeSpan _baseDelay;

    public EmailQueueWorker(
        IEmailQueue queue,
        IEmailSender sender,
        ILogger<EmailQueueWorker> logger,
        IConfiguration config)
    {
        _queue = queue;
        _sender = sender;
        _logger = logger;

        _maxRetries = config.GetValue<int?>("Email:MaxRetries") ?? 3;
        _baseDelay = TimeSpan.FromSeconds(config.GetValue<int?>("Email:BaseBackoffSeconds") ?? 2);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EmailQueueWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var item = await _queue.DequeueAsync(stoppingToken);
                if (item == null) continue;

                await HandleEmailAsync(item, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in worker loop.");
                await Task.Delay(2000, stoppingToken);
            }
        }

        _logger.LogInformation("EmailQueueWorker stopped.");
    }

    private async Task HandleEmailAsync(QueuedEmail email, CancellationToken ct)
    {
        int attempt = email.Retries + 1;

        try
        {
            await _sender.SendAsync(email.To, email.Subject, email.Body);
            _logger.LogInformation("Email sent to {To} (attempt {Attempt})", email.To, attempt);
        }
        catch (Exception ex) when (IsTransient(ex) && attempt < _maxRetries)
        {
            var delay = TimeSpan.FromSeconds(_baseDelay.TotalSeconds * Math.Pow(2, attempt));

            _logger.LogWarning("Retrying email to {To} in {Delay}s (attempt {Attempt}/{Max})",
                email.To, delay.TotalSeconds, attempt, _maxRetries);

            await Task.Delay(delay, ct);

            await _queue.EnqueueAsync(email with { Retries = attempt });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email FAILED permanently to {To} after {Attempts} attempts",
                email.To, attempt);
        }
    }

    private static bool IsTransient(Exception ex)
    {
        return ex is SocketException || ex is TimeoutException;
    }
}
