using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Test.Clients;
using Test.Repositories;

namespace Test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    public static AccountsService CreateServiceWithEmptyRepository(ICurrencyClient client = null!)
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, client);

        return service;
    }

    public static (AccountsService, Account, Account) CreateServiceWithTwoAccounts(
        decimal firstBalance = 0,
        decimal secondBalance = 0)
    {
        var (service, repository) = CreateServiceAndRepository();
        var firstAccount = new Account
        {
            HolderName = "Quux Q Quxerson",
            Balance = firstBalance,
            Id = "0",
        };
        var secondAccount = new Account
        {
            HolderName = "Corge C Corgeson",
            Balance = secondBalance,
            Id = "1"
        };
        
        repository.AddExistingAccount(firstAccount);
        repository.AddExistingAccount(secondAccount);

        return (service, firstAccount, secondAccount);
    }
    
    public static (AccountsService, Account) CreateServiceWithOneAccount(decimal balance = 0)
    {
        var (service, repository) = CreateServiceAndRepository();
        var account = new Account
        {
            HolderName = "Qux Q Quxson",
            Balance = balance,
            Id = "0"
        };
        repository.AddExistingAccount(account);

        return (service, account);
    }

    public static AccountsService CreateServiceWithThreeAccounts()
    {
        var (service, repository) = CreateServiceAndRepository();
        repository.AddExistingAccount(DummyAccounts.Foo);
        repository.AddExistingAccount(DummyAccounts.Bar);
        repository.AddExistingAccount(DummyAccounts.Baz);

        return service;
    }

    public static (AccountsService, Account) CreateServiceWithConversionDictionary()
    {
        var conversionDictionary = new Dictionary<string, decimal>()
        {
            { "fakeCurrency", 2 },
        };
        
        var currencyClient = new FakeCurrencyClient(conversionDictionary);
        var (service, repository) = CreateServiceAndRepository(currencyClient);
        var account = new Account
        {
            HolderName = "Qux Q Quxson",
            Balance = 1,
            Id = "0"
        };
        repository.AddExistingAccount(account);

        return (service, account);
    }

    public static AccountsService CreateServiceWithConversionDictionaryAndEmptyRepository()
    {
        var conversionDictionary = new Dictionary<string, decimal>()
        {
            { "fakeCurrency", 2 },
        };
        
        var currencyClient = new FakeCurrencyClient(conversionDictionary);
        var service = CreateServiceWithEmptyRepository(currencyClient);

        return service;
    }
    
    private static (AccountsService, FakeAccountRepository) CreateServiceAndRepository(
        ICurrencyClient? currencyClient = null)
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, currencyClient!);

        return (service, repository);
    }
}