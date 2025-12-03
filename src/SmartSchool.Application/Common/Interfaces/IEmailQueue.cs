namespace SmartSchool.Application.Common.Interfaces
{
    public interface IEmailQueue
    {
        /// <summary>
        /// Enqueue an email to be sent (fire-and-forget).
        /// </summary>
        Task EnqueueAsync(QueuedEmail message, CancellationToken ct = default);

        /// <summary>
        /// Current queue length (for monitoring).
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Try to dequeue, returns null if none available.
        /// </summary>
        Task<QueuedEmail?> DequeueAsync(CancellationToken ct);
    }

    public record QueuedEmail(string To, string Subject, string Body, int Retries = 0, string? CorrelationId = null);
}
