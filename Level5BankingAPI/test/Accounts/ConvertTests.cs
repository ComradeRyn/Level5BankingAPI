using System.Net;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class ConvertTests
{
    private readonly Account _account = new()
    {
        Id = "0",
        HolderName = "Foo F Foobert",
        Balance = 1
    };

    private readonly AccountsService _service;

    public ConvertTests()
    {
        var repository = new FakeAccountRepository(new Dictionary<string, Account>
        {
            { _account.Id, _account }
        });

        _service = AccountsTestHelpers.CreateService(repository);
    }
    
    [Fact]
    public async Task Convert_ToValidCurrencies_ReturnConvertedCurrencies()
    {
        // Arrange
        const string validConversionCurrency = "fakeCurrency1,fakeCurrency2";
        
        // Act
        var actual = await _service.Convert(
            new AccountRequest<ConversionRequest>(
                _account.Id, 
                new ConversionRequest(validConversionCurrency)));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equivalent(
            new ConversionResponse(new Dictionary<string, decimal>
            {
                { "fakeCurrency1", _account.Balance * 2 },
                { "fakeCurrency2", _account.Balance * 0.5m },
            }),
            actual.Content);
    }
    
    [Fact]
    public async Task Convert_ToInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidCurrency = "invalid";
        
        // Act
        var actual = await _service.Convert(
            new AccountRequest<ConversionRequest>(
                _account.Id, 
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
        
        // Act
        var actual = await _service.Convert(
            new AccountRequest<ConversionRequest>(
                nonexistentId,
                new ConversionRequest(validConversionRequest)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}