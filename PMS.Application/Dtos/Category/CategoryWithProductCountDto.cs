namespace PMS.Application.Dtos.Category;

public class CategoryWithProductCountDto
{
    public string Name { get; set; } = null!;
    public long Count { get; set; }
}