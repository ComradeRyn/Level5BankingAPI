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
        
        var expectedContent = new Application.DTOs.Account(
            account.Id, 
            account.HolderName, 
            1);
        
        // Act
        var actual = await service.Deposit(positiveAmountDepositRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        // Act
        var actual = await service.Deposit(zeroOrLessAccountRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
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
        
        // Act
        var actual = await service.Deposit(depositToNonExistentAccountRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}