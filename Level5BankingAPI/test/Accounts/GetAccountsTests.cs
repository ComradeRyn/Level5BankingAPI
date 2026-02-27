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
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Foo.AsDto(), 
                DummyAccounts.Bar.AsDto(), 
                DummyAccounts.Baz.AsDto()
            }, 
            actual.Content);
    }

    [Fact]
    public async Task Search_Name_ReturnAccountsWithName()
    {
        // Arrange
        const string nameWithMatchingResult = "Foo";
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                nameWithMatchingResult, 
                null, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Foo.AsDto(), 
            },
            actual.Content);
    }

    [Fact]
    public async Task Search_NameNoMatch_ReturnEmptyList()
    {
        // Arrange
        const string noMatchingName = "R";
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                noMatchingName, 
                null, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Sort_ByName_ReturnAccountListSortedByName()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "name", 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Bar.AsDto(),
                DummyAccounts.Baz.AsDto(),
                DummyAccounts.Foo.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByNameDescending_ReturnReversedAccountListByName()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "name", 
                true, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Foo.AsDto(),
                DummyAccounts.Baz.AsDto(),
                DummyAccounts.Bar.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalance_ReturnAccountListSortedByBalance()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "balance", 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Foo.AsDto(),
                DummyAccounts.Bar.AsDto(),
                DummyAccounts.Baz.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalanceDescending_ReturnReversedAccountListByBalance()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "balance", 
                true, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Baz.AsDto(),
                DummyAccounts.Bar.AsDto(),
                DummyAccounts.Foo.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByInvalidKeyword_ReturnFailure()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "invalid", 
                true, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Search_EmptyDatabase_ReturnEmptyList()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithEmptyRepository();
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Pagination_PageNumberZeroOrLess_ReturnPageNumberOneResults()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                0, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(1, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_ValidPageNumber_ReturnRequestedPage()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                2, 
                1));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(2, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_PageSizeZeroOrLess_ReturnPageSizeTenResults()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                null,
                null,
                false,
                1,
                0));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(10, paginationMetadata!.PageSize);
    }

    [Fact]
    public async Task Pagination_ValidPageSize_ReturnRequestedPageSize()
    {
        // Arrange
        var service = AccountsTestHelpers.CreateServiceWithThreeAccounts();
        
        var expectedContent = new List<Application.DTOs.Account>()
        {
            DummyAccounts.Foo.AsDto(),
            DummyAccounts.Bar.AsDto(),
        };
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                null,
                null,
                false,
                1,
                2));
        
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
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                "Ba",
                "name",
                true,
                1,
                1));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(1, paginationMetadata!.PageSize);
        Assert.Equal(
            new List<Application.DTOs.Account>()
            {
                DummyAccounts.Baz.AsDto(),
            },
            actual.Content);
    }
}