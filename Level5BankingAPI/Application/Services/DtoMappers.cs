using Account = Domain.Models.Account;
using DTOs_Account = Application.DTOs.Account;

namespace Application.Services;

public static class DtoMappers
{
    public static DTOs_Account AsDto(this Account account) 
        => new(account.Id, account.HolderName, account.Balance);

    public static IEnumerable<DTOs_Account> AsDto(this IEnumerable<Account> accounts)
        => accounts.Select(account => account.AsDto()).ToList();
}