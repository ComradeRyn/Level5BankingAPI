using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public class GetAccountsTests
{
    private readonly Account _fooAccount = new()
    {
        HolderName = "Foo F Foobert",
        Balance = 0,
        Id = "0"
    };
        
    private readonly Account _barAccount = new()
    {
        HolderName = "Bar B Babert",
        Balance = 1,
        Id = "1"
    };

    private readonly Account _bazAccount = new()
    {
        HolderName = "Baz B Bazert",
        Balance = 2,
        Id = "2"
    };
    
    [Fact]
    public async Task Search_NoArgs_ReturnAccountList()
    {
        // Arrange
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var noArgsRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _fooAccount.AsDto(), 
            _barAccount.AsDto(), 
            _bazAccount.AsDto()
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var onlyNamesContainingFooRequest = new GetAccountsRequest(
            "Foo", 
            null, 
            false, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _fooAccount.AsDto(), 
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var noMatchingNameRequest = new GetAccountsRequest(
            "R", 
            null, 
            false, 
            1, 
            10);
        SeedRepository(repository);

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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var sortByNameRequest = new GetAccountsRequest(
            null, 
            "name", 
            false, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _barAccount.AsDto(),
            _bazAccount.AsDto(),
            _fooAccount.AsDto(),
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var sortByNameDescendingRequest = new GetAccountsRequest(
            null, 
            "name", 
            true, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _fooAccount.AsDto(),
            _bazAccount.AsDto(),
            _barAccount.AsDto(),
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var sortByBalanceRequest = new GetAccountsRequest(
            null, 
            "balance", 
            false, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _fooAccount.AsDto(),
            _barAccount.AsDto(),
            _bazAccount.AsDto(),
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var sortByBalanceDescendingRequest = new GetAccountsRequest(
            null, 
            "balance", 
            true, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var expectedContent = new List<Application.DTOs.Account>()
        {
            _bazAccount.AsDto(),
            _barAccount.AsDto(),
            _fooAccount.AsDto(),
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var sortByInvalidKeywordRequest = new GetAccountsRequest(
            null, 
            "invalid", 
            true, 
            1, 
            10);
        SeedRepository(repository);

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
        var (service, _) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var pageNumberZeroRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            0, 
            10);
        SeedRepository(repository);

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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var validPageNumberRequest = new GetAccountsRequest(
            null, 
            null, 
            false, 
            2, 
            1);
        SeedRepository(repository);

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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var pageSizeZeroRequest = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            0);
        SeedRepository(repository);

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
        var (service, repository) = AccountsTestHelpers.CreateServiceAndEmptyRepository();
        var validPageSizeRequest = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            5);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageSize = 5;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(validPageSizeRequest);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageSize, paginationMetadata!.PageSize);
    }

    private void SeedRepository(FakeAccountRepository repository)
    {
        repository.AddExistingAccount(_fooAccount);
        repository.AddExistingAccount(_barAccount);
        repository.AddExistingAccount(_bazAccount);
    }
}