using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using test.Repositories;

namespace test.Accounts;

public class GetAccountsTests
{
    private readonly Account _fooAccount = new Account
    {
        HolderName = "Foo F Foobert",
        Balance = 0,
        Id = "0"
    };
        
    private readonly Account _barAccount = new Account
    {
        HolderName = "Bar B Babert",
        Balance = 1,
        Id = "1"
    };

    private readonly Account _bazAccount = new Account
    {
        HolderName = "Baz B Bazert",
        Balance = 2,
        Id = "2"
    };
    
    [Fact]
    public async Task Search_NoArgs_ReturnAccountList()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        // var actualStatusCode
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_Name_ReturnAccountsWithName()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Search_NameNoMatch_ReturnEmptyList()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            "Ryan", 
            null, 
            false, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        
        // Act
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Sort_ByName_ReturnAccountListSortedByName()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByNameDescending_ReturnReversedAccountListByName()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalance_ReturnAccountListSortedByBalance()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalanceDescending_ReturnReversedAccountListByBalance()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
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
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedContent, actual.Content);
    }

    [Fact]
    public async Task Sort_ByInvalidKeyword_ReturnFailure()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null, 
            "invalid", 
            true, 
            1, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        
        // Act
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Fact]
    public async Task Search_EmptyDatabase_ReturnEmptyList()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null, 
            null, 
            false, 
            1, 
            10);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        
        // Act
        var (actual, _) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Empty(actual.Content!);
    }

    [Fact]
    public async Task Pagination_PageNumberZeroOrLess_ReturnPageNumberOneResults()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null, 
            null, 
            false, 
            0, 
            10);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageNumber = 1;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageNumber, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_ValidPageNumber_ReturnRequestedPage()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null, 
            null, 
            false, 
            2, 
            1);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageNumber = 2;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageNumber, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task Pagination_PageSizeZeroOrLess_ReturnPageSizeOneResults()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            0);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageSize = 10;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(request);
        
        // Assert
        Assert.Equal(expectedStatusCode, actual.StatusCode);
        Assert.Equal(expectedPageSize, paginationMetadata!.PageSize);
    }

    [Fact]
    public async Task Pagination_ValidPageSize_ReturnRequestedPageSize()
    {
        // Arrange
        var repository = AccountsTestHelpers.CreateRepository();
        var service = AccountsTestHelpers.CreateService(repository);
        var request = new GetAccountsRequest(
            null,
            null,
            false,
            1,
            5);
        SeedRepository(repository);

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const int expectedPageSize = 5;
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(request);
        
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