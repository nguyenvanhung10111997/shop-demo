namespace example.order.infrastructure.Models;

public class PagingResult<T>
{
    public int TotalRecords { get; set; }
    public IEnumerable<T> Records { get; set; } = Enumerable.Empty<T>();
}