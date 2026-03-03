using Application.Services;
using Test.Clients;
using Test.Repositories;

namespace Test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    public static AccountsService CreateService(FakeAccountRepository repository) 
        => new(repository, CreateCurrencyClient());

    private static FakeCurrencyClient CreateCurrencyClient()
    {
        var conversionDictionary = new Dictionary<string, decimal>
        {
            { "fakeCurrency1", 2 },
            { "fakeCurrency2", .5m }
        };
        
        return new FakeCurrencyClient(conversionDictionary);
    }
}