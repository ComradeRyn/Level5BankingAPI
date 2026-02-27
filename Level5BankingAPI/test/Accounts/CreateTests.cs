using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class CreateTests
{
    [Fact]
    public async Task Create_AccountWithValidName_ReturnCreatedAccount()
    {
        // Arrange
        const string validName = "Foo F Foobert";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        var validNameRequest = new CreationRequest(validName);

        // Act
        var actual = await service.CreateAccount(validNameRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(new Application.DTOs.Account("0",validName,0), actual.Content);
    }

    [Fact]
    public async Task Create_AccountWithInvalidName_ReturnFailure()
    {
        // Arrange
        const string invalidName = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        var invalidNameRequest = new CreationRequest(invalidName);

        // Act
        var actual = await service.CreateAccount(invalidNameRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}