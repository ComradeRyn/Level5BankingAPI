using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class TransferTest
{
    [Fact]
    public async Task Transfer_PositiveLessThanOrEqualBalance_ReturnUpdatedReceiverAccount()
    {
        // Arrange
        const decimal accountOneBalance = 1;
        const decimal transferAmount = 1;
        var (service,
            sender,
            receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountOneBalance);

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
                0), 
            actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";
        var (service, sender) = AccountsTestHelpers.CreateServiceWithOneAccount();

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
        const decimal accountBalance = 1;
        const string nonExistentAccountId = "invalid";
        var (service, receiver) = AccountsTestHelpers.CreateServiceWithOneAccount(accountBalance);

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
        const decimal accountBalance = 1;
        var (service,
            sender,
            receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountBalance);

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
        const decimal accountBalance = 1;
        const decimal transferAmount = accountBalance + 1;
        var (service,
            sender,
            receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountBalance);

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