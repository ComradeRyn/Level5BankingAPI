using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public static class AccountsTestHelpers
{
    public static (AccountsService, FakeAccountRepository) CreateServiceAndEmptyRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, null!);

        return (service, repository);
    }

    public static (AccountsService, FakeAccountRepository, Account) CreateServiceAndPopulatedRepository()
    {
        var (service, repository) = CreateServiceAndEmptyRepository();
        var account = new Account
        {
            HolderName = "Foo F Foobert",
            Balance = 0,
            Id = "0",
        };
        
        repository.AddExistingAccount(account);

        return (service, repository, account);
    }
}