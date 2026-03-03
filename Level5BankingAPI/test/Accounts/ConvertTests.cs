using System.Net;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class ConvertTests
{
    [Fact]
    public async Task Convert_ToValidCurrencies_ReturnConvertedCurrencies()
    {
        // Arrange
        const string validConversionCurrency = "fakeCurrency1,fakeCurrency2";
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
        var actual = await service.Convert(
            new AccountRequest<ConversionRequest>(
                account.Id, 
                new ConversionRequest(validConversionCurrency)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equivalent(
            new ConversionResponse(new Dictionary<string, decimal>
            {
                { "fakeCurrency1", account.Balance * 2 },
                { "fakeCurrency2", account.Balance * 0.5m },
            }),
            actual.Content);
    }
    
    [Fact]
    public async Task Convert_ToInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidCurrency = "invalid";
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
        var actual = await service.Convert(
            new AccountRequest<ConversionRequest>(
                account.Id, 
                new ConversionRequest(invalidCurrency)));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Convert_NonexistentAccount_ReturnFailure()
    {
        // Arrange
        const string validConversionRequest = "fakeCurrency1";
        const string nonexistentId = "invalid";
        var accounts = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var actual = await service.Convert(
            new AccountRequest<ConversionRequest>(
                nonexistentId,
                new ConversionRequest(validConversionRequest)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}