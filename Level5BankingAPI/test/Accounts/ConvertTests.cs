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
        
        var expectedDictionary = new Dictionary<string, decimal>()
        {
            { "fakeCurrency", 2}
        };
        
        // Act
        var actual = await service.Convert(toValidCurrencyRequest);
        
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.True(TestDictionaryEquivalence(expectedDictionary, actual.Content!.ConvertedCurrencies));
    }
    
    [Fact]
    public async Task Convert_toInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidConversionCurrency = "invalidCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var conversionRequest = new ConversionRequest(invalidConversionCurrency);
        var toInvalidCurrencyRequest = new AccountRequest<ConversionRequest>(account.Id, conversionRequest);
        
        // Act
        var actual = await service.Convert(toInvalidCurrencyRequest);
        
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
        var conversionRequest = new ConversionRequest(validConversionRequest);
        var convertNonexistentAccountRequest = new AccountRequest<ConversionRequest>(nonexistentId, conversionRequest);
        
        // Act
        var actual = await service.Convert(convertNonexistentAccountRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    private static bool TestDictionaryEquivalence(
        Dictionary<string, decimal> dictionaryOne,
        Dictionary<string, decimal> dictionaryTwo)
    {
        var areKeysEqual = dictionaryOne.Keys.ToList()[0] == dictionaryTwo.Keys.ToList()[0];
        var areValuesEqual = dictionaryOne.Values.ToList()[0] == dictionaryTwo.Values.ToList()[0];
        
        return areValuesEqual && areKeysEqual;
    }
}