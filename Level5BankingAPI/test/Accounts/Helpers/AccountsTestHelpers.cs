using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts.Helpers;

public static class AccountsTestHelpers
{
    public static AccountsService CreateServiceWithEmptyRepository()
    {
        var (service, _) = CreateServiceAndRepository();

        return service;
    }
    
    public static (AccountsService, Account) CreateServiceWithSingleAccount(
        decimal balance = 0)
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
    
    private static (AccountsService, FakeAccountRepository) CreateServiceAndRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, null!);

        return (service, repository);
    }
}