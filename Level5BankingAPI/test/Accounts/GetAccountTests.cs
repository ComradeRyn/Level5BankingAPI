using System.Net;
using Application.Services;

namespace test.Accounts;

public class GetAccountTests
{
    [Fact]
    public async Task Get_NonExistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonIncludedId = "no account";
        var (service, _) = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        
        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        
        // Act
        var actual = await service.GetAccount(nonIncludedId);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Get_ExistentAccount_ReturnFoundAccount()
    {
        // Arrange
        var (service, account) = AccountsTestHelpers.CreateServiceWithAccountInRepository();

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = account.AsDto();
        
        // Act
        var actual = await service.GetAccount(account.Id);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }
}