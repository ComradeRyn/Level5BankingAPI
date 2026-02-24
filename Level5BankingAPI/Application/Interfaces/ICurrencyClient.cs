using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ICurrencyClient
{
    Task<CurrencyClientResponse> GetConversionRates(string currencyTypes);
}