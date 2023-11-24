namespace Mapify;

public class PagedResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    public PagedResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalCount">The total count.</param>
    public PagedResult(List<T> data, int pageNumber, int pageSize, int totalCount) : this()
    {
        Data = data ?? new List<T>();
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public List<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Gets a value indicating whether this instance has a previous page.
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether this instance has a next page.
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;
}
