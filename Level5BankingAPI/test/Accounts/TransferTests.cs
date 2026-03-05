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
    
    private readonly Account _sender;
    private readonly Account _receiver;
    private readonly AccountsService _service;

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

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { _sender.Id, _sender },
                { _receiver.Id, _receiver }
            });

        _service = AccountsTestHelpers.CreateService(repository);
    }
    
    [Fact]
    public async Task Transfer_PositiveLessThanOrEqualBalance_ReturnUpdatedReceiverAccount()
    {
        // Arrange
        const decimal transferAmount = 1;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                transferAmount, 
                _sender.Id, 
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                _sender.Id,
                _sender.HolderName,
                SenderBalance - transferAmount), 
            actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";
       
        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                transferAmount,
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
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";
        
        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                transferAmount, 
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
        const decimal transferAmount = 0;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                transferAmount, 
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
        const decimal transferAmount = 2;

        // Act
        var actual = await _service.Transfer(
            new TransferRequest(
                transferAmount, 
                _sender.Id, 
                _receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}