using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public static class AccountsTestHelpers
{
    public static (AccountsService, FakeAccountRepository) CreateServiceWithEmptyRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, null!);

        return (service, repository);
    }
    
    public static (AccountsService, Account) CreateServiceWithPopulatedRepository(
        decimal balance = 0)
    {
        var (service, repository) = CreateServiceWithEmptyRepository();
        var account = new Account
        {
            HolderName = "Foo F Foobert",
            Balance = balance,
            Id = "0",
        };
        
        repository.AddExistingAccount(account);

        return (service, account);
    }
}