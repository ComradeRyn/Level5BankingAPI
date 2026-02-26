using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Account = Domain.Models.Account;

namespace Test.Repositories;

public class FakeAccountRepository : IAccountsRepository
{
    private readonly Dictionary<string, Account> _accounts;
    private int _currentId;

    public FakeAccountRepository(Dictionary<string, Account> accounts)
    {
        _accounts = accounts;
    }
    
    public Task<(IEnumerable<Account>, PaginationMetadata)> GetAccounts(
        string? name, 
        string? sortBy, 
        bool isDescending, 
        int pageNumber, 
        int pageSize)
    {
        var query = _accounts.Values.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.Trim();
            query = query.Where(account => account.HolderName.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            Expression<Func<Account, object>> sortPattern = sortBy switch
            {
                "name" => account => account.HolderName,
                "balance" => account => account.Balance,
                _ => account => account.Id
            };

            query = isDescending
                ? query.OrderByDescending(sortPattern)
                : query.OrderBy(sortPattern);
        }
        
        var itemCount = query.Count();
        var paginationMetadata = new PaginationMetadata(pageNumber,
            pageSize,
            itemCount);
        
        var accounts = query.Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IEnumerable<Account>, PaginationMetadata)>((accounts, paginationMetadata));
    }

    public Task<Account?> GetAccount(string id)
    {
        _accounts.TryGetValue(id, out var account);
        
        return Task.FromResult(account);
    }

    public Task<Account> AddAccount(string name)
    {
        var account = new Account
        {
            Id = _currentId.ToString(),
            HolderName = name,
        };
        
        _accounts.Add(account.Id, account);
        _currentId++;

        return Task.FromResult(account);
    }

    public Task<Account> UpdateAccount(Account account, decimal amount)
    {
        account.Balance += amount;

        return Task.FromResult(account);
    }

    public void AddExistingAccount(Account account)
        => _accounts.Add(account.Id, account);
}