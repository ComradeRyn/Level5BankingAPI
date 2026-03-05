using Application.Services;
using Domain.Models;
using Test.Clients;
using Test.Repositories;

namespace Test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    public static AccountsService CreateService(FakeAccountRepository repository)
    {
        var conversionDictionary = new Dictionary<string, decimal>
        {
            { "fakeCurrency1", 2 },
            { "fakeCurrency2", .5m }
        };
        
        return new AccountsService(repository, new FakeCurrencyClient(conversionDictionary));
    }
}