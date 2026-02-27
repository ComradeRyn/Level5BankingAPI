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
        
        var transferPositiveInboundsAmountRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);
        
        var expectedContent = new Application.DTOs.Account(
            sender.Id,
            sender.HolderName,
            0);

        // Act
        var actual = await service.Transfer(transferPositiveInboundsAmountRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";
        var (service, sender) = AccountsTestHelpers.CreateServiceWithOneAccount();
        var transferFromNonexistentAccountRequest = new TransferRequest(
            transferAmount,
            sender.Id,
            nonExistentAccountId);

        // Act
        var actual = await service.Transfer(transferFromNonexistentAccountRequest);

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
        var transferToNonexistentAccountRequest = new TransferRequest(
            transferAmount, 
            nonExistentAccountId,
            receiver.Id);

        // Act
        var actual = await service.Transfer(transferToNonexistentAccountRequest);

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
        
        var transferZeroOrLessRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);

        // Act
        var actual = await service.Transfer(transferZeroOrLessRequest);

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
        
        var transferLargerThanBalanceRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);

        // Act
        var actual = await service.Transfer(transferLargerThanBalanceRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}