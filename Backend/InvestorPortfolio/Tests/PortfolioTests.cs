using Core.Entities;
using Core.Entities.Auth;
using Core.Entities.SpecificData;

namespace Tests;

public class PortfolioTests
{
    private Portfolio _portfolio;
    public PortfolioTests()
    {
        var guid = Guid.NewGuid();
        _portfolio = new Portfolio()
        {
            Accounts = new HashSet<BrokerageAccount>(),
            Id = guid,
            Owner = new User()
            {
                Id = guid,
                UserName = "TestUser",
                PasswordHash = "TestPasswordHash",
                Email = "TestEmail"
                
            },
        };
    }
    
    [Fact]
    public void TestAddAccount()
    {
        // Arrange
        

        // Act
        _portfolio.TryAddAccount(title: "TestAccount", out var account);

        // Assert
        Assert.Contains(account, _portfolio.Accounts!);
    }

    [Fact]
    public void TestRemoveAccount()
    {
        // Arrange
        _portfolio.TryAddAccount("TestAccount", out var account);

        // Act
        _portfolio.DeleteAccount(account!.Title, out var result);

        // Assert
        Assert.DoesNotContain(account, _portfolio.Accounts);
        Assert.Equivalent(result, account);
    }

    [Fact]
    public void TestGetBalanceWithSameCurrencyCode()
    {
        // Arrange
        var acc1 = new BrokerageAccountBuilder(_portfolio, "TestAccount").WithBonds(new HashSet<StockContainer<Bond>>
        {
            new(
                new Bond
                {
                    Nominal = new MoneyValue()
                    {
                        Currency = new CurrencyCode("USD"),
                        Units = 1000,
                        Nano = 0
                    },
                    Lot = 10
                },
                10)
        }).Build();
        var acc2 = new BrokerageAccountBuilder(_portfolio, "TestAccount").WithShares(new HashSet<StockContainer<Share>>
        {
            new(
                new Share
                {
                    Nominal = new MoneyValue()
                    {
                        Currency = new CurrencyCode("USD"),
                        Units = 1000,
                        Nano = 0
                    },
                    Lot = 100
                },
                15)
        }).Build();
        _portfolio.TryAddAccount(acc1, out var res1);
        _portfolio.TryAddAccount(acc2, out var res2);

        // Act
        var totalValue = _portfolio.GetPricesByCurrenciesTickers().Sum(b => b.Value);

        // Assert
        Assert.Equal(100_000, totalValue);
    }
}