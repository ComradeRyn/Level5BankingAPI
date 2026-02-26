using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using test.Accounts.Helpers;
using test.Repositories;

namespace test.Accounts;

public class GetAccountsTests
{
    [Fact]
    public async Task Search_NoArgs_ReturnAllAccounts()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var noArgsRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(), 
            DummyAccounts.Bar.AsDto(), 
            DummyAccounts.Baz.AsDto()
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(noArgsRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_Name_ReturnAccountsWithName()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var onlyNamesContainingFooRequest = new GetAccountsRequest(
            "Foo", 
            null, 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(), 
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(onlyNamesContainingFooRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_NameNoMatch_ReturnEmptyList()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var noMatchingNameRequest = new GetAccountsRequest(
            "R", 
            null, 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        
        // Act
        var (actual, _) = await service.GetAccounts(noMatchingNameRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Sort_ByName_ReturnAccountListSortedByName()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var sortByNameRequest = new GetAccountsRequest(
            null, 
            "name", 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Foo.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByNameRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByNameDescending_ReturnReversedAccountListByName()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var sortByNameDescendingRequest = new GetAccountsRequest(
            null, 
            "name", 
            true, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Bar.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByNameDescendingRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalance_ReturnAccountListSortedByBalance()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var sortByBalanceRequest = new GetAccountsRequest(
            null, 
            "balance", 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Baz.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByBalanceRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalanceDescending_ReturnReversedAccountListByBalance()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var sortByBalanceDescendingRequest = new GetAccountsRequest(
            null, 
            "balance", 
            true, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Foo.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByBalanceDescendingRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByInvalidKeyword_ReturnFailure()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var sortByInvalidKeywordRequest = new GetAccountsRequest(
            null, 
            "invalid", 
            true, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByInvalidKeywordRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Search_EmptyDatabase_ReturnEmptyList()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        var searchEmptyDatabaseRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        
        // Act
        var (actual, _) = await service.GetAccounts(searchEmptyDatabaseRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Pagination_PageNumberZeroOrLess_ReturnPageNumberOneResults()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var pageNumberZeroRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            0, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageNumber = 1;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(pageNumberZeroRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageNumber, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_ValidPageNumber_ReturnRequestedPage()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var validPageNumberRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            2, 
            1);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageNumber = 2;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageNumberRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageNumber, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_PageSizeZeroOrLess_ReturnPageSizeOneResults()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var pageSizeZeroRequest = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            0);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageSize = 10;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(pageSizeZeroRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageSize, paginationMetadata!.PageSize);
    }

    [Fact]
    public async Task Pagination_ValidPageSize_ReturnRequestedPageSize()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var validPageSizeRequest = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            5);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageSize = 5;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageSizeRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageSize, paginationMetadata!.PageSize);
    }
}