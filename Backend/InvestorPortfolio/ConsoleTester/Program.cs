using Application.Integration.Tinkoff;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

// Setup
var token = Environment.GetEnvironmentVariable("TOKEN");
ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

var serviceCollection = new ServiceCollection();
serviceCollection.AddInvestmentTinkoffClient((provider, settings) =>
{
    settings.AccessToken = token;
});
var serviceProvider = serviceCollection.BuildServiceProvider();


// Using
var stockExchange = serviceProvider.GetRequiredService<IStockExchange>();

var bonds = await stockExchange.GetBondsAsync();
var currencies = await stockExchange.GetCurrenciesAsync();
var shares = await stockExchange.GetSharesAsync();

var portfolio = new Portfolio();
portfolio.TryAddAccount(title: "TestAccount", out var account);

foreach (var value in shares)
{
    Console.WriteLine("Name: {0} | Ticker: {1} | PlacementPrice: {2} | Lot: {3}", value.Name, value.Ticker, value.Nominal, value.Lot);
}