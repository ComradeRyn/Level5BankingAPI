namespace Application.DTOs.Requests;

public record GetAccountsRequest(
    string? Name,
    string? SortBy,
    bool IsDescending,
    int PageNumber,
    int PageSize);