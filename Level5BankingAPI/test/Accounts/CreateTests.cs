using System.Net;
using Application.DTOs.Requests;
using Application.Services;
using Domain.Models;
using Test.Accounts.Helpers;
using Test.Repositories;

namespace Test.Accounts;

public class CreateTests
{
    private readonly AccountsService _service;

    public CreateTests()
    {
        _service = AccountsTestHelpers.CreateService(new FakeAccountRepository(
            new Dictionary<string, Account>()));
    }
    
    [Fact]
    public async Task Create_AccountWithValidName_ReturnCreatedAccount()
    {
        // Arrange
        const string validName = "Foo F Foobert";

        // Act
        var actual = await _service.CreateAccount(new CreationRequest(validName));

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(
            new Application.DTOs.Account(
                "0",
                validName, 
                0), 
            actual.Content);
    }

    [Fact]
    public async Task Create_AccountWithInvalidName_ReturnFailure()
    {
        // Arrange
        const string invalidName = "invalid";

        // Act
        var actual = await _service.CreateAccount(new CreationRequest(invalidName));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.Null(actual.Content);
    }
}