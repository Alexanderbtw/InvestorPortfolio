using Core.Entities;
using Core.Entities.SpecificData;

namespace Tests;

public class PortfolioTests
{
    [Fact]
    public void TestAddAccount()
    {
        // Arrange
        var portfolio = new Portfolio();

        // Act
        portfolio.TryAddAccount(title: "TestAccount", out var account);

        // Assert
        Assert.Contains(account, portfolio.Accounts);
    }

    [Fact]
    public void TestRemoveAccount()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.TryAddAccount("TestAccount", out var account);

        // Act
        portfolio.DeleteAccount(account.Title, out var result);

        // Assert
        Assert.DoesNotContain(account, portfolio.Accounts);
        Assert.Equivalent(result, account);
    }

    [Fact]
    public void TestGetBalanceWithSameCurrencyCode()
    {
        // Arrange
        var portfolio = new Portfolio();
        var acc1 = new BrokerageAccountBuilder(portfolio, "TestAccount").WithBonds(new HashSet<StockContainer<Bond>>
        {
            new(
                new Bond("TestBond", "TestIsin", new MoneyValue(new CurrencyCode("USD"), 1000, 0), 10, "TestName"),
                10)
        }).Build();
        var acc2 = new BrokerageAccountBuilder(portfolio, "TestAccount").WithShares(new HashSet<StockContainer<Share>>
        {
            new(
                new Share("TestBond", "TestIsin", new MoneyValue(new CurrencyCode("USD"), 100, 0), 100, "TestName2"),
                15)
        }).Build();
        portfolio.TryAddAccount(acc1, out var res1);
        portfolio.TryAddAccount(acc2, out var res2);

        // Act
        var totalValue = portfolio.Balance.Sum(b => b.Value);

        // Assert
        Assert.Equal(100_000, totalValue);
    }
}