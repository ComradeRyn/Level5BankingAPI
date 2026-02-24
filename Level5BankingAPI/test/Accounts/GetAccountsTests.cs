using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public class GetAccountsTests
{
    [Fact]
    public async Task Create_AccountWithValidName_ReturnCreatedAccount()
    {
        // Arrange
        const string validName = "Ryan L Yuncza";
        var service = CreateService();
        var request = new CreationRequest(validName);
        
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new Application.DTOs.Account("0",validName,0);

        // Act
        var actual = await service.CreateAccount(request);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Create_AccountWithInvalidName_ReturnFailure()
    {
        // Arrange
        const string invalidName = "invalid";
        var service = CreateService();
        var request = new CreationRequest(invalidName);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        var expectedContent = null as Application.DTOs.Account;

        // Act
        var actual = await service.CreateAccount(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    private static AccountsService CreateService()
    {
        var accountsDictionary = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accountsDictionary);
        
        return new AccountsService(repository, null!);
    }
}