using System.Net;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class GetAccountTests
{
    [Fact]
    public async Task Get_NonExistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonIncludedId = "invalid";
        var accounts = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var actual = await service.GetAccount(nonIncludedId);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Get_ExistentAccount_ReturnFoundAccount()
    {
        // Arrange
        var account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var accounts = new Dictionary<string, Account>()
        {
            { account.Id, account}
        };
        
        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var actual = await service.GetAccount(account.Id);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(account.AsDto(), actual.Content);
    }
}