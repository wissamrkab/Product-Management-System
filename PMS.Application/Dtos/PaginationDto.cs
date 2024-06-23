namespace PMS.Application.Dtos;

public class PaginationDto<T>
{
    public long TotalRecords  { get; set; }
    public long PageSize   { get; set; }
    public long CurrentPage    { get; set; }
    public long TotalPages     { get; set; }
    public T Data { get; set; } = default!;
}