using Application.DTOs;
using Account = Domain.Models.Account;

namespace Application.Interfaces;

public interface IAccountsRepository
{
    Task<(IEnumerable<Account>, PaginationMetadata)> GetAccounts(string? name, 
        string? sortBy, 
        bool isDescending,
        int pageNumber, 
        int pageSize);
    Task<Account?> GetAccount(string id);
    Task<Account> AddAccount(string name);
    Task<Account> UpdateAccount(Account account, decimal amount);
}