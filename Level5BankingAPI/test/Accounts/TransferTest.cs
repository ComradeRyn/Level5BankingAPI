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

        var (service, sender, receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountOneBalance);
        var transferPositiveInboundsAmountRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new Application.DTOs.Account(
            sender.Id, 
            sender.HolderName, 
            sender.Balance - transferAmount);

        // Act
        var actual = await service.Transfer(transferPositiveInboundsAmountRequest);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Transfer_FromNonexistentAccount_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 1;
        const string nonExistentAccountId = "invalid";

        var (service, sender) = AccountsTestHelpers.CreateServiceWithOneAccount();
        var transferFromNonexistentAccountRequest = new TransferRequest(
            transferAmount,
            sender.Id,
            nonExistentAccountId);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

        // Act
        var actual = await service.Transfer(transferFromNonexistentAccountRequest);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_ToNonexistentAccount_ReturnFailure()
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

        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

        // Act
        var actual = await service.Transfer(transferToNonexistentAccountRequest);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_ZeroOrLess_ReturnFailure()
    {
        // Arrange
        const decimal transferAmount = 0;
        const decimal accountBalance = 1;

        var (service, sender, receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountBalance);
        var transferZeroOrLessRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

        // Act
        var actual = await service.Transfer(transferZeroOrLessRequest);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Transfer_LargerThanSenderBalance_ReturnFailure()
    {
        // Arrange
        const decimal accountBalance = 1;
        const decimal transferAmount = accountBalance + 1;

        var (service, sender, receiver) = AccountsTestHelpers.CreateServiceWithTwoAccounts(accountBalance);
        var transferLargerThanBalanceRequest = new TransferRequest(
            transferAmount, 
            sender.Id, 
            receiver.Id);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

        // Act
        var actual = await service.Transfer(transferLargerThanBalanceRequest);

        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}