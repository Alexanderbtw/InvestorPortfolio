using System.Text.Json;
using Core.Entities;
using Core.Entities.Auth;
using Core.Entities.SpecificData;
using Persistence.FileSavers;

namespace Tests;

public class FileSavingTests
{
    private Portfolio _portfolio;

    public FileSavingTests()
    {
        var ulid = Guid.NewGuid();
        _portfolio = new Portfolio()
        {
            Accounts = new HashSet<BrokerageAccount>(),
            Id = ulid,
            Owner = new User()
            {
                Id = ulid,
                UserName = "TestUser",
                PasswordHash = "TestPasswordHash",
                Email = "TestEmail"
                
            },
        };
        _portfolio.TryAddAccount(title: "TestAccount", out var account);
        account?.AddStock(new Share
        {
            Nominal = new MoneyValue()
            {
                Currency = new CurrencyCode("USD"),
                Units = 1000,
                Nano = 0
            },
            Lot = 10
        }, 10, out _);
    }
    
    [Fact]
    public void JsonSave()
    {
        // Arrange
        
        
        // Act
        var saver = new JsonSaver<Portfolio>();
        saver.Save(_portfolio, "Portfolio.json");

        // Assert
        Assert.True(File.Exists("Portfolio.json"));
    }

    [Fact]
    public void JsonRecovery()
    {
        // Arrange
        File.WriteAllText("Portfolio.json", JsonSerializer.Serialize(_portfolio));

        // Act
        var saver = new JsonSaver<Portfolio>();
        var result = saver.Recovery("Portfolio.json");

        var obj1Str = JsonSerializer.Serialize(_portfolio);
        var obj2Str = JsonSerializer.Serialize(result);

        // Assert

        Assert.Equal(obj1Str, obj2Str);
    }
}