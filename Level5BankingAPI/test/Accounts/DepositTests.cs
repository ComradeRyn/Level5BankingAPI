using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class DepositTests
{
    private const decimal AccountBalance = 1;
    private readonly Account _account;
    private readonly AccountsService _service;

    public DepositTests()
    {
        _account = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = AccountBalance,
        };
        
        var repository = new FakeAccountRepository(new Dictionary<string, Account>
        {
            { _account.Id, _account }
        });

        _service = AccountsTestHelpers.CreateService(repository);
    }
    
    [Fact]
    public async Task Deposit_PositiveAmount_ReturnUpdatedAccount()
    {
        // Arrange
        const decimal positiveAmount = 1;
        
        // Act
        var actual = await _service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                _account.Id, 
                new ChangeBalanceRequest(positiveAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                _account.Id, 
                _account.HolderName, 
                AccountBalance + positiveAmount),
            actual.Content);
    }

    [Fact]
    public async Task Deposit_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal zeroOrLessAmount = 0;
        
        // Act
        var actual = await _service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                _account.Id, 
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
        
        // Act
        var actual = await _service.Deposit(
            new AccountRequest<ChangeBalanceRequest>(
                nonExistentAccountId, 
                new ChangeBalanceRequest(positiveAmount)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}