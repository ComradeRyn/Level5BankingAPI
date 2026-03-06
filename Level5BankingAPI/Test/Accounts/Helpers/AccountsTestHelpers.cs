using Application.Services;
using Test.Clients;
using Test.Repositories;

namespace Test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    private static readonly Dictionary<string, decimal> ConversionDictionary = new()
    {
        { "fakeCurrency1", 2 },
        { "fakeCurrency2", .5m }
    };
    
    public static AccountsService CreateService(FakeAccountRepository repository)
        => new (repository, new FakeCurrencyClient(ConversionDictionary));
}