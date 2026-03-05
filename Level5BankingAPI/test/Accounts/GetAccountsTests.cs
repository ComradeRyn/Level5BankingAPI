using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class GetAccountsTests
{
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

    private readonly AccountsService _service;
    
    public GetAccountsTests()
    {
        var repository = new FakeAccountRepository(new Dictionary<string, Account>
        {
            { _fooAccount.Id, _fooAccount },
            { _barAccount.Id, _barAccount },
            { _bazAccount.Id, _bazAccount }
        });

        _service = AccountsTestHelpers.CreateService(repository);
    }
    
    [Fact]
    public async Task GetAccounts_SortByNull_ReturnAccountList()
    {
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                null, 
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
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "name", 
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
        // Act
        var (actual, _) = await _service.GetAccounts(
            new GetAccountsRequest(
                null, 
                "balance", 
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
        // Act
        var (actual, _) = await _service.GetAccounts(
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
    public async Task GetAccounts_PageNumberZeroOrLess_ReturnPageNumberOne()
    {
        // Act
        var (actual ,paginationMetadata) = await _service.GetAccounts(
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
    public async Task GetAccounts_ValidPageNumber_ReturnRequestedPage()
    {
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
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
    public async Task GetAccounts_PageSizeZeroOrLess_ReturnPageSizeTen()
    { 
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
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
    public async Task GetAccounts_ValidPageSize_ReturnRequestedPageSize()
    {
        // Act
        var (actual,paginationMetadata) = await _service.GetAccounts(
            new GetAccountsRequest(
                null,
                null,
                false,
                1,
                2));
        
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