using System.Net;
using Application.DTOs.Requests;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class TransferTests
{
    [Fact]
    public async Task Transfer_PositiveLessThanOrEqualBalance_ReturnUpdatedReceiverAccount()
    {
        // Arrange
        const decimal transferAmount = 1;
        const decimal senderBalance = 1;
        var sender = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = senderBalance
        };

        var receiver = new Account
        {
            Id = "1",
            HolderName = "Bar B Barson",
            Balance = 1
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { sender.Id, sender },
                { receiver.Id, receiver }
            });
        
        var service = AccountsTestHelpers.CreateService(repository);

        // Act
        var actual = await service.Transfer(
            new TransferRequest(
                transferAmount, 
                sender.Id, 
                receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                sender.Id,
                sender.HolderName,
                senderBalance - transferAmount), 
            actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";
        var sender = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { sender.Id, sender },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);

        // Act
        var actual = await service.Transfer(
            new TransferRequest(
                transferAmount,
                sender.Id,
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

        var receiver = new Account
        {
            Id = "1",
            HolderName = "Bar B Barson",
            Balance = 1
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { receiver.Id, receiver }
            });
        
        var service = AccountsTestHelpers.CreateService(repository);

        // Act
        var actual = await service.Transfer(
            new TransferRequest(
                transferAmount, 
                nonExistentAccountId,
                receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 0;
        const decimal senderBalance = 1;
        var sender = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = senderBalance
        };

        var receiver = new Account
        {
            Id = "1",
            HolderName = "Bar B Barson",
            Balance = 1
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { sender.Id, sender },
                { receiver.Id, receiver }
            });
        
        var service = AccountsTestHelpers.CreateService(repository);

        // Act
        var actual = await service.Transfer(
            new TransferRequest(
                transferAmount, 
                sender.Id, 
                receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_LargerThanSenderBalance_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 2;
        const decimal senderBalance = 1;
        var sender = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = senderBalance
        };

        var receiver = new Account
        {
            Id = "1",
            HolderName = "Bar B Barson",
            Balance = 1
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>()
            {
                { sender.Id, sender },
                { receiver.Id, receiver }
            });
        
        var service = AccountsTestHelpers.CreateService(repository);

        // Act
        var actual = await service.Transfer(
            new TransferRequest(
                transferAmount, 
                sender.Id, 
                receiver.Id));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}