using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class GetAccountsTests
{
    private readonly AccountsService _service;
    private readonly Account _fooAccount = new()
    {
        Id = "0",
        HolderName = "Foo F Foobert",
        Balance = 1
    };

    private readonly Account _barAccount = new()
    {
        Id = "1",
        HolderName = "Bar B Babert",
        Balance = 1,
    };

    private readonly Account _bazAccount = new()
    {
        Id = "2",
        HolderName = "Baz B Bazert",
        Balance = 2,
    };
    
    public GetAccountsTests()
    {
        _service = AccountsTestHelpers.CreateService(new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { _fooAccount.Id, _fooAccount },
                { _barAccount.Id, _barAccount },
                { _bazAccount.Id, _bazAccount }
            }));
    }
    
    [Fact]
    public async Task GetAccounts_SortByNull_ReturnAccountList()
    {
        // Arrange
        const string? sortBy = null;
        
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                sortBy, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                _fooAccount.AsDto(),
                _barAccount.AsDto(),
                _bazAccount.AsDto(),
            },
            actual.Content);
    }
    
    [Fact]
    public async Task GetAccounts_SortByName_ReturnAccountListSortedByName()
    {
        // Arrange
        const string sortBy = "name";
        
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                sortBy, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                _barAccount.AsDto(),
                _bazAccount.AsDto(),
                _fooAccount.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task GetAccounts_SortByBalance_ReturnAccountListSortedByBalance()
    {
        // Arrange
        const string sortBy = "balance";
        
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                sortBy, 
                false, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                _fooAccount.AsDto(),
                _barAccount.AsDto(),
                _bazAccount.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task GetAccounts_ByInvalidKeyword_ReturnFailure()
    {
        // Arrange
        const string sortBy = "invalid";
        
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                sortBy, 
                true, 
                1, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAccounts_PageNumberZeroOrLess_ReturnPageNumberOne(int pageNumber)
    {
        // Act
        var (actual ,paginationMetadata) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                pageNumber, 
                10));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(1, paginationMetadata!.CurrentPage);
    }

    [Fact]
    public async Task GetAccounts_ValidPageNumber_ReturnRequestedPage()
    {
        // Arrange
        const int validPageNumber = 2;
        
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
                false, 
                validPageNumber, 
                1));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(2, paginationMetadata!.CurrentPage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAccounts_PageSizeZeroOrLess_ReturnPageSizeTen(int pageSize)
    { 
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
            new GetAccountsRequest(
                null,
                null,
                false,
                1,
                pageSize));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(10, paginationMetadata!.PageSize);
    }

    [Fact]
    public async Task GetAccounts_ValidPageSize_ReturnRequestedPageSize()
    {
        // Arrange
        const int validPageSize = 2;
        
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
            new GetAccountsRequest(
                null,
                null,
                false,
                1,
                validPageSize));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(2, paginationMetadata!.PageSize);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                _fooAccount.AsDto(),
                _barAccount.AsDto(),
            },
            actual.Content);
    }
}