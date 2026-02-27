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

        // Act
        var actual = await service.CreateAccount(new CreationRequest(validName));

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                "0",
                validName, 
                0), 
            actual.Content);
    }

    [Fact]
    public async Task Create_AccountWithInvalidName_ReturnFailure()
    {
        // Arrange
        const string invalidName = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();

        // Act
        var actual = await service.CreateAccount(new CreationRequest(invalidName));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}