using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Account = Domain.Models.Account;

namespace Infrastructure.Repositories;

public class AccountsRepository : IAccountsRepository
{
    private readonly AccountContext _context;

    public AccountsRepository(AccountContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Account>, PaginationMetadata)> GetAccounts(string? name, 
        string? sortBy, 
        bool isDescending, 
        int pageNumber, 
        int pageSize)
    {
        var query = _context.Accounts as IQueryable<Account>;
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
        
        var itemCount = await query.CountAsync();
        var paginationMetadata = new PaginationMetadata(pageNumber,
            pageSize,
            itemCount);
        
        var accounts = await query.Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (accounts, paginationMetadata);
    }

    public async Task<Account?> GetAccount(string id)
        => await _context.Accounts.FindAsync(id);

    public async Task<Account> AddAccount(string name)
    {
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            HolderName = name,
        };
        
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account> UpdateAccount(Account account, decimal amount)
    {
        account.Balance += amount;
        await _context.SaveChangesAsync();

        return account;
    }
}