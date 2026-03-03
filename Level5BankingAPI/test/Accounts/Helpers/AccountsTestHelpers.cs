using Application.Services;
using Domain.Models;
using Test.Clients;
using Test.Repositories;

namespace Test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    public static AccountsService CreateService()
    {
        var accountsDictionary = new Dictionary<string, Account>
        {
            { DummyAccounts.Foo.Id, DummyAccounts.Foo },
            { DummyAccounts.Bar.Id, DummyAccounts.Bar },
            { DummyAccounts.Baz.Id, DummyAccounts.Baz }
        };
        var repository = new FakeAccountRepository(accountsDictionary);

        return new AccountsService(repository, CreateCurrencyClient());
    }

    public static AccountsService CreateServiceWithEmptyRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);

        return new AccountsService(repository, CreateCurrencyClient());
    }

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