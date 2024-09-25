namespace Hermes.Infrastructure.Pagination;
public class PaginatedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public long TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => CurrentPage >= 1;
    public bool HasNextPage => CurrentPage + 1 < TotalPages;

    public PaginatedList() { }
    public PaginatedList(IEnumerable<T> source, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source.Skip(CurrentPage * PageSize).Take(PageSize));
    }

    public PaginatedList(IEnumerable<T> source, int currentPage, int pageSize, long totalCount)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source);
    }
}