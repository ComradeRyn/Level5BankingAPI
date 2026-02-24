using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public static class AccountsTestHelpers
{
    public static (AccountsService, FakeAccountRepository) CreateServiceAndRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        var service = new AccountsService(repository, null!);

        return (service, repository);
    }
}