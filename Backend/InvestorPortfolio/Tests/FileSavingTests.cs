using System.Text.Json;
using Core.Entities;
using Core.Entities.SpecificData;

namespace Tests;

public class FileSavingTests
{
    [Fact]
    public void JsonSave()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.TryAddAccount(title: "TestAccount", out var account);
        account.AddShares(new Share("isin", "ticker", new MoneyValue(new CurrencyCode("USD"), 1000, 0), 10, "name"), 10);
        
        // Act
        var saver = new JsonSaver<Portfolio>();
        saver.Save(portfolio, "Portfolio.json");

        // Assert
        Assert.True(File.Exists("Portfolio.json"));
    }

    [Fact]
    public void JsonRecovery()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.TryAddAccount(title: "TestAccount", out var account);
        account.AddShares(new Share("isin", "ticker", new MoneyValue(new CurrencyCode("USD"), 1000, 0), 10, "name"), 10);
        File.WriteAllText("Portfolio.json", JsonSerializer.Serialize(portfolio));

        // Act
        var saver = new JsonSaver<Portfolio>();
        var result = saver.Recovery("Portfolio.json");

        var obj1Str = JsonSerializer.Serialize(portfolio);
        var obj2Str = JsonSerializer.Serialize(result);
        Assert.Equal(obj1Str, obj2Str);

        // Assert

        // Assert.(portfolio, result);
    }
}