using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class GetAccountsTests
{
    [Fact]
    public async Task Sort_ByNull_ReturnAccountList()
    {
        // Arrange
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(),
                barAccount.AsDto(),
                bazAccount.AsDto(),
            },
            actual.Content);
    }
    
    [Fact]
    public async Task Sort_ByName_ReturnAccountListSortedByName()
    {
        // Arrange
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                barAccount.AsDto(),
                bazAccount.AsDto(),
                fooAccount.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByBalance_ReturnAccountListSortedByBalance()
    {
        // Arrange
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 0
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(),
                barAccount.AsDto(),
                bazAccount.AsDto(),
            },
            actual.Content);
    }

    [Fact]
    public async Task Sort_ByInvalidKeyword_ReturnFailure()
    {
        // Arrange
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
    public async Task Pagination_PageNumberZeroOrLess_ReturnPageNumberOneResults()
    {
        // Arrange
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var (actual ,paginationMetadata) = await service.GetAccounts(
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
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
        var fooAccount = new Account
        {
            Id = "0",
            HolderName = "Foo F Foobert",
            Balance = 1
        };
        
        var barAccount = new Account
        {
            Id = "1",
            HolderName = "Bar B Babert",
            Balance = 1,
        };

        var bazAccount = new Account
        {
            Id = "2",
            HolderName = "Baz B Bazert",
            Balance = 2,
        };

        var repository = new FakeAccountRepository(
            new Dictionary<string, Account>
            {
                { fooAccount.Id, fooAccount },
                { barAccount.Id, barAccount },
                { bazAccount.Id, bazAccount },
            });
        
        var service = AccountsTestHelpers.CreateService(repository);
        
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
        Assert.Equal(2, paginationMetadata!.PageSize);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(),
                barAccount.AsDto(),
            },
            actual.Content);
    }
}