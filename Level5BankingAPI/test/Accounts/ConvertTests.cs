using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class ConvertTests
{
    [Fact]
    public async Task Convert_toValidCurrency_ReturnConvertedCurrencies()
    {
        // Arrange
        const string validConversionCurrency = "fakeCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var conversionRequest = new ConversionRequest(validConversionCurrency);
        var toValidCurrencyRequest = new AccountRequest<ConversionRequest>(account.Id, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedConversion = new KeyValuePair<string, decimal>("fakeCurrency", 2);
        
        // Act
        var actual = await service.Convert(toValidCurrencyRequest);
        
        // Assert
        var actualContainsKey = actual.Content!.ConvertedCurrencies.ContainsKey(expectedConversion.Key);
        var actualContainsValue = actual.Content!.ConvertedCurrencies.ContainsValue(expectedConversion.Value);
        
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.True(actualContainsValue && actualContainsKey);
    }
    
    [Fact]
    public async Task Convert_toInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidConversionCurrency = "invalidCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var conversionRequest = new ConversionRequest(invalidConversionCurrency);
        var toInvalidCurrencyRequest = new AccountRequest<ConversionRequest>(account.Id, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var actual = await service.Convert(toInvalidCurrencyRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Convert_NonexistentAccount_ReturnFailure()
    {
        // Arrange
        const string validConversionRequest = "fakeCurrency";
        const string nonexistentId = "invalid";

        var service = AccountsTestHelpers.CreateServiceWithConversionDictionaryAndEmptyRepository();
        var conversionRequest = new ConversionRequest(validConversionRequest);
        var convertNonexistentAccountRequest = new AccountRequest<ConversionRequest>(nonexistentId, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        
        // Act
        var actual = await service.Convert(convertNonexistentAccountRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}