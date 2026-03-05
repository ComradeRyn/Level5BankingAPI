using System.Net;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class GetAccountTests
{
    private readonly Account _account = new()
    {
        Id = "0",
        HolderName = "Foo F Foobert",
        Balance = 1
    };

    private readonly AccountsService _service;

    public GetAccountTests()
    {
        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { _account.Id, _account }
            });

        _service = AccountsTestHelpers.CreateService(repository);
    }
    
    [Fact]
    public async Task GetAccount_NonExistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonIncludedId = "invalid";
        
        // Act
        var actual = await _service.GetAccount(nonIncludedId);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task GetAccount_ExistentAccount_ReturnFoundAccount()
    {
        // Act
        var actual = await _service.GetAccount(_account.Id);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(_account.AsDto(), actual.Content);
    }
}