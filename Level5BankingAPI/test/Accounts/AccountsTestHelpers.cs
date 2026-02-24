using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public static class AccountsTestHelpers
{
    public static AccountsService CreateService()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        
        return new AccountsService(repository, null!);
    }

    public static AccountsService CreateService(FakeAccountRepository repository)
        => new AccountsService(repository, null!);

    public static FakeAccountRepository CreateRepository()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        
        return new FakeAccountRepository(accountsDictionary);
    }
}