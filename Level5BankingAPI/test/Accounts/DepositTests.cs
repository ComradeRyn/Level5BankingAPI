using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class DepositTests
{
    [Fact]
    public async Task Deposit_PositiveAmount_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal positiveAmount = 1;
        var changeBalanceRequest = new ChangeBalanceRequest(positiveAmount);
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount();
        var positiveAmountDepositRequest = new AccountRequest<ChangeBalanceRequest>(account.Id, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new Application.DTOs.Account(
            account.Id, 
            account.HolderName, 
            account.Balance + positiveAmount);
        
        // Act
        var actual = await service.Deposit(positiveAmountDepositRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Deposit_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal zeroOrLessAmount = -1;
        var changeBalanceRequest = new ChangeBalanceRequest(zeroOrLessAmount);
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount();
        var zeroOrLessAccountRequest = new AccountRequest<ChangeBalanceRequest>(account.Id, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var actual = await service.Deposit(zeroOrLessAccountRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Deposit_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal positiveAmount = 1;
        const string nonExistentAccountId = "invalid";
        var changeBalanceRequest = new ChangeBalanceRequest(positiveAmount);
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        var depositToNonExistentAccountRequest = 
            new AccountRequest<ChangeBalanceRequest>(nonExistentAccountId, changeBalanceRequest);
        
        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        
        // Act
        var actual = await service.Deposit(depositToNonExistentAccountRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}