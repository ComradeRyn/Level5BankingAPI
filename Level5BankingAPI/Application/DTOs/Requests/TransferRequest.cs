namespace Application.DTOs.Requests;

public record TransferRequest(decimal Amount, 
    string SenderId, 
    string ReceiverId);