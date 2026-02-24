namespace Application.DTOs.Requests;

public record AccountRequest<T>(string Id, T Content);