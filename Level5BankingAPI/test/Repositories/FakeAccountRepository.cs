using Application.DTOs;
using Application.Interfaces;
using Account = Domain.Models.Account;

namespace test.Repositories;

public class FakeAccountRepository : IAccountsRepository
{
    public Task<(IEnumerable<Account>, PaginationMetadata)> GetAccounts(
        string? name, 
        string? sortBy, 
        bool isDescending, 
        int pageNumber, 
        int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Account?> GetAccount(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Account> AddAccount(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Account> UpdateAccount(Account account, decimal amount)
    {
        throw new NotImplementedException();
    }
}