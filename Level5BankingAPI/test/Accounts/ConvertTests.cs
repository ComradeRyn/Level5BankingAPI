using System.Net;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class ConvertTests
{
    [Fact]
    public async Task Convert_toValidCurrency_ReturnConvertedCurrencies()
    {
        // Arrange
        const string validConversionCurrency = "fakeCurrency1,fakeCurrency2";
        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        
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
                { "fakeCurrency1", 2 },
                { "fakeCurrency2", 0.5m },
            }),
            actual.Content);
    }
    
    [Fact]
    public async Task Convert_toInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidConversionCurrency = "invalidCurrency";
        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        
        // Act
        var actual = await service.Convert(
            new AccountRequest<ConversionRequest>(
                account.Id, 
                new ConversionRequest(invalidConversionCurrency)));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Convert_NonexistentAccount_ReturnFailure()
    {
        // Arrange
        const string validConversionRequest = "fakeCurrency";
        const string nonexistentId = "invalid";
        var service = AccountsTestHelpers.CreateServiceWithConversionDictionaryAndEmptyRepository();
        
        // Act
        var actual = await service.Convert(
            new AccountRequest<ConversionRequest>(nonexistentId, 
                new ConversionRequest(validConversionRequest)));
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}