using System.Net;
using Application.DTOs.Requests;
using Test.Accounts.Helpers;

namespace Test.Accounts;

public class ConvertTests
{
    [Fact]
    public async Task Receive_ValidStatusCode_ReturnConversion()
    {
        // Arrange
        const string validConversionRequest = "fakeCurrency";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var conversionRequest = new ConversionRequest(validConversionRequest);
        var yieldsValidStatusCodeRequest = new AccountRequest<ConversionRequest>(account.Id, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedConversion = new KeyValuePair<string, decimal>("fakeCurrency", 2);
        
        // Act
        var actual = await service.Convert(yieldsValidStatusCodeRequest);
        
        // Assert
        var containsKey = actual.Content!.ConvertedCurrencies.ContainsKey(expectedConversion.Key);
        var containsValue = actual.Content!.ConvertedCurrencies.ContainsValue(expectedConversion.Value);
        
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.True(containsValue && containsKey);
    }
    
    [Fact]
    public async Task Receive_InvalidStatusCode_ReturnFailure()
    {
        // Arrange
        const string invalidConversionRequest = "invalid";

        var (service, account) = AccountsTestHelpers.CreateServiceWithConversionDictionary();
        var conversionRequest = new ConversionRequest(invalidConversionRequest);
        var yieldsInvalidStatusCodeRequest = new AccountRequest<ConversionRequest>(account.Id, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var actual = await service.Convert(yieldsInvalidStatusCodeRequest);
        
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

        var service = AccountsTestHelpers.CreateServiceWithConversionDictionaryAndNoAccount();
        var conversionRequest = new ConversionRequest(validConversionRequest);
        var yieldsInvalidStatusCodeRequest = new AccountRequest<ConversionRequest>(nonexistentId, conversionRequest);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        
        // Act
        var actual = await service.Convert(yieldsInvalidStatusCodeRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}