using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Test.Accounts.Helpers;

namespace Test.Accounts;

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
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(), 
            DummyAccounts.Bar.AsDto(), 
            DummyAccounts.Baz.AsDto()
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(noArgsRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_Name_ReturnAccountsWithName()
    {
        // Arrange
        const string nameWithMatchingResult = "Foo";
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var onlyNamesContainingFooRequest = new GetAccountsRequest(
            nameWithMatchingResult, 
            null, 
            false, 
            1, 
            10);
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(), 
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(onlyNamesContainingFooRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_NameNoMatch_ReturnEmptyList()
    {
        // Arrange
        const string noMatchingName = "R";
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var noMatchingNameRequest = new GetAccountsRequest(
            noMatchingName, 
            null, 
            false, 
            1, 
            10);
        
        // Act
        var (actual, _) = await service.GetAccounts(noMatchingNameRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Foo.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByNameRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Bar.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByNameDescendingRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Baz.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByBalanceRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Baz.AsDto(),
            DummyAccounts.Bar.AsDto(),
            DummyAccounts.Foo.AsDto(),
        };
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByBalanceDescendingRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        // Act
        var (actual, _) = await service.GetAccounts(sortByInvalidKeywordRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
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
        
        // Act
        var (actual, _) = await service.GetAccounts(searchEmptyDatabaseRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
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
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(pageNumberZeroRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(1, paginationMetadata!.CurrentPage);
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
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageNumberRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(2, paginationMetadata!.CurrentPage);
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
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(pageSizeZeroRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(10, paginationMetadata!.PageSize);
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
            2);
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Bar.AsDto(),
        };
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageSizeRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
        Assert.Equal(2, paginationMetadata!.PageSize);
    }

    [Fact]
    public async Task Pagination_isDescendingList_ReturnsCorrespondingContent()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        var validPageSizeRequest = new GetAccountsRequest(
            "Ba",
            "name",
            true,
            1,
            1);
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Baz.AsDto(),
        };
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageSizeRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
        Assert.Equal(1, paginationMetadata!.PageSize);
    }
}