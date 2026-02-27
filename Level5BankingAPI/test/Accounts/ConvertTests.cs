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
        const string validConversionCurrency = "fakeCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var toValidCurrencyRequest = new AccountRequest<ConversionRequest>(
            account.Id, 
            new ConversionRequest(validConversionCurrency));
        
        var expectedContent = new ConversionResponse(new Dictionary<string, decimal>()
        {
            { "fakeCurrency", 2 }
        });
        
        // Act
        var actual = await service.Convert(toValidCurrencyRequest);
        
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equivalent(expectedContent, actual.Content);
    }
    
    [Fact]
    public async Task Convert_toInvalidCurrency_ReturnFailure()
    {
        // Arrange
        const string invalidConversionCurrency = "invalidCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var toInvalidCurrencyRequest = new AccountRequest<ConversionRequest>(account.Id, 
            new ConversionRequest(invalidConversionCurrency));
        
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
        var convertNonexistentAccountRequest = new AccountRequest<ConversionRequest>(nonexistentId, 
            new ConversionRequest(validConversionRequest));
        
        // Act
        var actual = await service.Convert(convertNonexistentAccountRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}