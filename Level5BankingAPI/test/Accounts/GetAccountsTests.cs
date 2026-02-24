using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public class GetAccountsTests
{
    [Fact]
    public async Task Get_NonExistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonIncludedId = "no account";
        var service = AccountsTestHelpers.CreateService();
        
        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        var expectedContent = null as Application.DTOs.Account;
        
        // Act
        var actual = await service.GetAccount(nonIncludedId);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Get_ExistentAccount_ReturnFoundAccount()
    {
        // Arrange
        var account = new Account
        {
            HolderName = "Ryan L Yuncza",
            Balance = 0,
            Id = "0"
        };
        
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        repository.AddExistingAccount(account);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new Application.DTOs.Account(account.Id, account.HolderName, 0);
        
        // Act
        var actual = await service.GetAccount(account.Id);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }
}