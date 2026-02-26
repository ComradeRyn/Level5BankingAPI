using Application.Services;
using Microsoft.Extensions.Configuration;

namespace Test.Authentication;

public class AuthenticationTests
{
    [Fact]
    public void Create_Token_ReturnToken()
    {
        // Arrange
        var service = new AuthenticationService(BuildConfiguration());
        
        // Act
        var actual = service.CreateToken();
        
        // Assert
        Assert.NotNull(actual.Token);
    }

    private static IConfiguration BuildConfiguration()
    {
        var authenticationDetails = new Dictionary<string, string>
        {
            {
                "Authentication:SecretForKey",
                "VGhpcyBpcyBhIHNlY3JldCBwYXNzd29yZCB3aGljaCB3aWxsIGJlIHVzZWQgdG8gZW5jb2RlIHNlY3JldCB0aGluZ3M="
            },
            { "Authentication:Issuer", "http://localhost:5079" },
            { "Authentication:Audience", "bankingapi" }
        };
        
        return new ConfigurationBuilder()
            .AddInMemoryCollection(authenticationDetails!)
            .Build();
    }
}