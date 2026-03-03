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
    public async Task Search_DefaultArgs_ReturnAllAccounts()
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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
                bazAccount.AsDto()
            }, 
            actual.Content);
    }

    [Fact]
    public async Task Search_Name_ReturnAccountsWithName()
    {
        // Arrange
        const string nameWithMatchingResult = "Foo";
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(), 
            },
            actual.Content);
    }

    [Fact]
    public async Task Search_NameWithNoMatch_ReturnEmptyList()
    {
        // Arrange
        const string nameWithNoMatch = "R";
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var (actual, _) = await service.GetAccounts(
            new GetAccountsRequest(
                nameWithNoMatch, 
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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
    public async Task Sort_ByNameDescending_ReturnReversedAccountListByName()
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(),
                bazAccount.AsDto(),
                barAccount.AsDto(),
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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
    public async Task Sort_ByBalanceDescending_ReturnReversedAccountListByBalance()
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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
            new List<Application.DTOs.Account>
            {
                bazAccount.AsDto(),
                barAccount.AsDto(),
                fooAccount.AsDto(),
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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
    public async Task Search_EmptyDatabase_ReturnEmptyList()
    {
        // Arrange
        var accounts = new Dictionary<string, Account>();
        var repository = new FakeAccountRepository(accounts);
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
        Assert.Empty(actual.Content!);
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
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

    [Fact]
    public async Task Pagination_isDescendingList_ReturnsCorrespondingContent()
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

        var accounts = new Dictionary<string, Account>
        {
            { fooAccount.Id, fooAccount },
            { barAccount.Id, barAccount },
            { bazAccount.Id, bazAccount },
        };

        var repository = new FakeAccountRepository(accounts);
        var service = AccountsTestHelpers.CreateService(repository);
        
        // Act
        var (actual,paginationMetadata) = await service.GetAccounts(
            new GetAccountsRequest(
                null,
                "name",
                true,
                1,
                1));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(1, paginationMetadata!.PageSize);
        Assert.Equal(
            new List<Application.DTOs.Account>
            {
                fooAccount.AsDto(),
            },
            actual.Content);
    }
}