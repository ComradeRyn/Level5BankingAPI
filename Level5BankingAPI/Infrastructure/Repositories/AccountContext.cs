using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; init; }
}