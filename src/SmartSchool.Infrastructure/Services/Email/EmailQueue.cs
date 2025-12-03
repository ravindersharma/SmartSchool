using Serilog;
using SmartSchool.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace SmartSchool.Infrastructure.Services.Email
{
    public class EmailQueue : IEmailQueue, IDisposable
    {
        private readonly ConcurrentQueue<QueuedEmail> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);
        private bool _disposed;

        public int Count => _queue.Count;

        public Task EnqueueAsync(QueuedEmail message, CancellationToken ct = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(EmailQueue));
            _queue.Enqueue(message);
            _signal.Release();
            Log.Information("Enqueued email to {To}. Queue size {Count}", message.To, _queue.Count);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Try to dequeue, returns null if none available.
        /// </summary>
        public async Task<QueuedEmail?> DequeueAsync(CancellationToken ct)
        {
            try
            {
                await _signal.WaitAsync(ct);
            }
            catch (OperationCanceledException) { return null; }

            if (_queue.TryDequeue(out var item))
                return item;

            return null;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _signal.Dispose();
            _disposed = true;
        }
    }
}
