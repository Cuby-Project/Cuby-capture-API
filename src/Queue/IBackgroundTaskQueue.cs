namespace Cuby.Queue
{
    /// <summary>
    /// Interface for a background task queue
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Queue a background work item
        /// </summary>
        /// <param name="workItem"></param>
        /// <returns></returns>
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, string> workItem);

        /// <summary>
        /// Dequeue a background work item
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<Func<CancellationToken, string>> DequeueAsync(CancellationToken cancellationToken);
    }
}
