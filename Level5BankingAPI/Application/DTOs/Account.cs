using Application.Interfaces;

namespace Application.DTOs;

public record Account(
    string Id,
    string HolderName,
    decimal Amount) : ICsvFormatter
{
    public string FormatCsv()
        => $"{Id},{HolderName},{Amount}\n";

    public string CreateHeader()
        => "Id,HolderName,Amount\n";
}