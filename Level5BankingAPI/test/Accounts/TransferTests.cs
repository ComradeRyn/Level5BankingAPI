using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class TransferTests
{
    private const decimal SenderBalance = 1;
    
    private readonly AccountsService _service;
    private readonly Account _sender;
    private readonly Account _receiver;

    public TransferTests()
    {
        _sender = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = SenderBalance
        };

        _receiver = new Account
        {
            Id = "1",
            HolderName = "Bar B Barson",
            Balance = 1
        };

        _service = AccountsTestHelpers.CreateService(new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { _sender.Id, _sender },
                { _receiver.Id, _receiver }
            }));
    }
    
    [Fact]
    public async Task Transfer_PositiveLessThanOrEqualBalance_ReturnUpdatedReceiverAccount()
    {
        // Arrange
        const decimal validAmount = 1;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                validAmount, 
                _sender.Id, 
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                _sender.Id,
                _sender.HolderName,
                SenderBalance - validAmount), 
            actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonExistentAccountId = "invalid";
       
        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                1,
                _sender.Id,
                nonExistentAccountId));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_FromNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const string nonExistentAccountId = "invalid";
        
        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                1, 
                nonExistentAccountId,
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal zeroOrLessAmount = 0;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                zeroOrLessAmount, 
                _sender.Id, 
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_LargerThanSenderBalance_ReturnFailure()
    {
        // Arrange
        const decimal largerThanBalanceAmount = 2;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                largerThanBalanceAmount, 
                _sender.Id, 
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}