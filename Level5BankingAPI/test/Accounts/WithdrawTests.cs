using System.Net;
using Application.DTOs.Requests;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class WithdrawTests
{
    [Fact]
    public async Task Withdraw_PositiveLessThanOrEqualBalance_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal accountBalance = 1;
        const decimal withdrawAmount = 1;
        var account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = accountBalance
        };
        
        var accounts = new Dictionary<string, Account>()
        {
            { account.Id, account }
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
                accountBalance - withdrawAmount), 
            actual.Content);
    }

    [Fact]
    public async Task Withdraw_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = 0;
        var account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var accounts = new Dictionary<string, Account>()
        {
            { account.Id, account }
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
        var accounts = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
        var account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = accountBalance
        };
        
        var accounts = new Dictionary<string, Account>()
        {
            { account.Id, account }
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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