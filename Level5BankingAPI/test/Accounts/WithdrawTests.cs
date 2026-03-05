using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class WithdrawTests
{
    private const decimal AccountBalance = 1;
    private readonly Account _account;
    private readonly AccountsService _service;

    public WithdrawTests()
    {
        _account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = AccountBalance
        };

        _service = AccountsTestHelpers.CreateService(new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { _account.Id, _account }
            }));
    }
    
    [Fact]
    public async Task Withdraw_PositiveLessThanOrEqualBalance_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal withdrawAmount = 1;
        
        // Act
        var actual = await _service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                _account.Id,
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                _account.Id, 
                _account.HolderName, 
                AccountBalance - withdrawAmount), 
            actual.Content);
    }

    [Fact]
    public async Task Withdraw_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal withdrawAmount = 0;
        
        // Act
        var actual = await _service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                _account.Id,
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
        
        // Act
        var actual = await _service.Withdraw(
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
        const decimal withdrawAmount = 2;
        
        // Act
        var actual = await _service.Withdraw(
            new AccountRequest<ChangeBalanceRequest>(
                _account.Id,
                new ChangeBalanceRequest(withdrawAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}