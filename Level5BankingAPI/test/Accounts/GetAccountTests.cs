using System.Net;
using Application.Services;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class GetAccountTests
{
    [Fact]
    public async Task Get_NonExistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonIncludedId = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        
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
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount();
        
        // Act
        var actual = await service.GetAccount(account.Id);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(account.AsDto(), actual.Content);
    }
}