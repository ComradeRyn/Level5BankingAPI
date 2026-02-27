using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class WithdrawTests
{
    [Fact]
    public async Task Withdraw_PositiveLessThanOrEqualBalance_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal accountBalance = 1;
        const decimal withdrawAmount = 1;
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount(accountBalance);
        
        // Act
        var actual = await service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                account.Id,
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                account.Id, 
                account.HolderName, 
                0), 
            actual.Content);
    }

    [Fact]
    public async Task Withdraw_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = -1;
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount();
        
        // Act
        var actual = await service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                account.Id,
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
    
    [Fact]
    public async Task Withdraw_FromNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = 1;
        const string nonExistentAccountId = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        
        // Act
        var actual = await service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                nonExistentAccountId, 
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
    
    [Fact]
    public async Task Withdraw_GreaterThanBalance_ReturnFailure()
    {
        // Arrange
        const decimal accountBalance = 1;
        const decimal withdrawAmount = 2;
        var (service, account) = AccountsTestHelpers.CreateServiceWithOneAccount(accountBalance);
        
        // Act
        var actual = await service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                account.Id,
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}