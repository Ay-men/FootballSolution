namespace Application.Common.Paging;

public abstract class PagedResultBase
{
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }

    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < PageCount;
}