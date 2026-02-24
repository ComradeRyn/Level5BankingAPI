using Application.Interfaces;

namespace Application.DTOs.Responses;

public record TokenResponse(string Token) : ICsvFormatter
{
    public string FormatCsv()
        => $"{Token}\n";

    public string CreateHeader()
        => "Token\n";
}