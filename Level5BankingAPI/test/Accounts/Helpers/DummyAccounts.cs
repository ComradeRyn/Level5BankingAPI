using Domain.Models;

namespace test.Accounts.Helpers;

public static class DummyAccounts
{
    public static readonly Account Foo = new()
    {
        HolderName = "Foo F Foobert",
        Balance = 0,
        Id = "0"
    };
        
    public static readonly Account Bar = new()
    {
        HolderName = "Bar B Babert",
        Balance = 1,
        Id = "1"
    };

    public static readonly Account Baz = new()
    {
        HolderName = "Baz B Bazert",
        Balance = 2,
        Id = "2"
    };
}