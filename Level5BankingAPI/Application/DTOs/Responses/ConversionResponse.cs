using System.Text;
using Application.Interfaces;

namespace Application.DTOs.Responses;

public record ConversionResponse(Dictionary<string, decimal> ConvertedCurrencies) : ICsvFormatter
{
    public string FormatCsv()
    {
        var buffer = new StringBuilder();
        foreach (var keyValuePair in ConvertedCurrencies)
        {
            buffer.Append($"{keyValuePair.Key},{keyValuePair.Value}\n");
        }

        return buffer.ToString();
    }

    public string CreateHeader()
        => "CurrencyType,Value\n";
}