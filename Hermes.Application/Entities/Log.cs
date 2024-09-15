namespace Hermes.Application.Entities;
public class ElasticLog
{
    public string Message { get; set; } = string.Empty;
    public string Created { get; set; } = string.Empty;
}

public class PaginatedLogsResponse
{
    public List<ElasticLog> Logs { get; set; } = new List<ElasticLog>();
    public long TotalCount { get; set; }
}