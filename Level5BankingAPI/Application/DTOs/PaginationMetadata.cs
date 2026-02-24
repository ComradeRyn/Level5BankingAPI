namespace Application.DTOs;

public record PaginationMetadata(int CurrentPage, int PageSize, int TotalItemCount)
{
    public int TotalPageCount => (int)Math.Ceiling(TotalItemCount / (double)PageSize);
}