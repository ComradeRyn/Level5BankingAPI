using System.Net;
using Application.DTOs.Requests;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class DepositTests
{
    [Fact]
    public async Task Deposit_PositiveAmount_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal positiveAmount = 1;
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
        var actual = await service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                account.Id, 
                new ChangeBalanceRequest(positiveAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                account.Id, 
                account.HolderName, 
                account.Balance),
            actual.Content);
    }

    [Fact]
    public async Task Deposit_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal zeroOrLessAmount = -1;
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
        var actual = await service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                account.Id, 
                new ChangeBalanceRequest(zeroOrLessAmount)));
        
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
        var accounts = new Dictionary<string, Account>();

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var actual = await service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                nonExistentAccountId, 
                new ChangeBalanceRequest(positiveAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}