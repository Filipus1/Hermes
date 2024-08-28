using Hermes.Infrastructure.Dto;

namespace Hermes.Infrastructure.CronJobs.Manager;
public class CronJobManager : ICronJobManager
{
    private const int MAX_QUEUE_LENGTH = 10;
    private readonly Queue<ServerDataDto> _jobQueue;

    public CronJobManager(Queue<ServerDataDto> jobQueue)
    {
        _jobQueue = jobQueue;
    }

    public Queue<ServerDataDto> GetQueue() => _jobQueue;

    public void EnqueueJob(ServerDataDto job)
    {
        if (_jobQueue.Count == MAX_QUEUE_LENGTH)
        {
            _jobQueue.Dequeue();
        }

        _jobQueue.Enqueue(job);
    }
}

public interface ICronJobManager
{
    void EnqueueJob(ServerDataDto job);
    Queue<ServerDataDto> GetQueue();
}