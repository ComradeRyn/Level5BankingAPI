using System.Net;
using Application.DTOs.Responses;
using Application.Interfaces;

namespace Test.Clients;

public class FakeCurrencyClient : ICurrencyClient
{
    private readonly Dictionary<string, decimal> _conversionRates;

    public FakeCurrencyClient(Dictionary<string, decimal> conversionRates)
    {
        _conversionRates = conversionRates;
    }
    
    public Task<CurrencyClientResponse> GetConversionRates(string currencyTypes)
    {
        var conversionRatesToReturn = new Dictionary<string, decimal>();
        foreach (var currencyType in currencyTypes.Split(','))
        {
            if (!_conversionRates.ContainsKey(currencyTypes))
            {
                var response = new CurrencyClientResponse(HttpStatusCode.BadRequest, 
                    "Could not find requested conversion rate", 
                    null);

                return Task.FromResult(response);
            }
            
            conversionRatesToReturn.Add(currencyType, _conversionRates[currencyType]);
        }

        return Task.FromResult(new CurrencyClientResponse(conversionRatesToReturn));
    }
}