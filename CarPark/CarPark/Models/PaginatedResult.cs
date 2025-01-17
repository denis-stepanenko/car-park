namespace CarPark.Models
{
    public record PaginatedResult<T>(List<T> Items, int PageNumber, int TotalPages) where T : BaseModel;
}
