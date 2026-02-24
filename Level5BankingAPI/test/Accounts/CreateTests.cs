using System.Net;
using Application.DTOs.Requests;

namespace test.Accounts;

public class CreateTests
{
    [Fact]
    public async Task Create_AccountWithValidName_ReturnCreatedAccount()
    {
        // Arrange
        const string validName = "Ryan L Yuncza";
        var (service, _) = AccountsTestHelpers.CreateServiceAndRepository();
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
        var (service, _) = AccountsTestHelpers.CreateServiceAndRepository();
        var request = new CreationRequest(invalidName);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

        // Act
        var actual = await service.CreateAccount(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}