using System.Net;
using Application.DTOs.Requests;
using test.Accounts.Helpers;

namespace test.Accounts;

public class WithdrawTests
{
    [Fact]
    public async Task Withdraw_PositiveLessThanOrEqualBalance_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal accountBalance = 1;
        var (service, account) = AccountsTestHelpers.CreateServiceWithSingleAccount(accountBalance);
        var changeBalanceRequest = new ChangeBalanceRequest(accountBalance);
        var positiveAmountWithinBoundsRequest = new AccountRequest<ChangeBalanceRequest>
            (account.Id, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new Application.DTOs.Account(
            account.Id, 
            account.HolderName, 
            0);
        
        // Act
        var actual = await service.Withdraw(positiveAmountWithinBoundsRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Withdraw_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = -1;
        var (service, account) = AccountsTestHelpers.CreateServiceWithSingleAccount(withdrawAmount);
        var changeBalanceRequest = new ChangeBalanceRequest(withdrawAmount);
        var amountZeroOrLessRequest = new AccountRequest<ChangeBalanceRequest>
            (account.Id, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var actual = await service.Withdraw(amountZeroOrLessRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
    
    [Fact]
    public async Task Withdraw_FromNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = 1;
        const string nonExistentAccountId = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        var changeBalanceRequest = new ChangeBalanceRequest(withdrawAmount);
        var validRequest = new AccountRequest<ChangeBalanceRequest>
            (nonExistentAccountId, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        
        // Act
        var actual = await service.Withdraw(validRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
    
    [Fact]
    public async Task Withdraw_GreaterThanBalance_ReturnFailure()
    {
        // Arrange
        const decimal accountBalance = 1;
        const decimal withdrawAmount = accountBalance + 1;
        var (service, account) = AccountsTestHelpers.CreateServiceWithSingleAccount(accountBalance);
        var changeBalanceRequest = new ChangeBalanceRequest(withdrawAmount);
        var validRequest = new AccountRequest<ChangeBalanceRequest>
            (account.Id, changeBalanceRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var actual = await service.Withdraw(validRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}