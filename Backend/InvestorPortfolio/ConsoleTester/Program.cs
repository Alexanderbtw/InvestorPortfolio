using Application.Integration.Tinkoff;
using Application.Integrations.Tinkoff;
using Core.Entities;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Core.Interfaces;
using Infrastructure.Converters.CurrencyApi;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Persistence.FileSavers;
using Persistence.Interfaces;

// Setup
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables()
    .Build();

string? token = configuration["TOKEN"];
ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));
string? apikey = configuration["APIKEY"];
ArgumentException.ThrowIfNullOrEmpty(apikey, nameof(apikey));

var serviceCollection = new ServiceCollection();
serviceCollection.AddInvestmentTinkoffClient((provider, settings) =>
{
    settings.AccessToken = token;
});
serviceCollection.AddHttpClient<CurrencyConvertApiClient, CurrencyApiClient>(httpClient => {
    httpClient.DefaultRequestHeaders.Add("apikey", apikey);
});
serviceCollection.AddTransient<ICurrencyConverter<MoneyValue, CurrencyCode>, CurrencyConverter>();
serviceCollection.AddSingleton(typeof(IAsyncFileSaver<>), typeof(JsonSaver<>)); 
serviceCollection.AddSingleton(typeof(IFileSaver<>), typeof(XmlSaver<>));






// Using
var serviceProvider = serviceCollection.BuildServiceProvider();
var portfolio = new Portfolio();
var stockExchange = serviceProvider.GetRequiredService<IStockExchange>();
var converter = serviceProvider.GetRequiredService<ICurrencyConverter<MoneyValue, CurrencyCode>>();

portfolio.TryAddAccount(title: "TestAccount", out var account);
portfolio["TestAccount"]?.AddShares(new Share("isin", "ticker", new MoneyValue(new CurrencyCode("USD"), 1000, 0), 10, "name"), 10);
portfolio["TestAccount"]?.AddShares(new Share("isin", "ticker", new MoneyValue(new CurrencyCode("USD"), 100, 0), 1, "name"), 10);

var val = portfolio["TestAccount"]!.Shares.First(x => x.LastStock.Name == "name")!.SumPurchaseValue;
var res = await converter.ConvertAsync(val, new CurrencyCode("RUB"));
Console.WriteLine(res);

var saver = serviceProvider.GetRequiredService<IAsyncFileSaver<Portfolio>>();

await saver.SaveAsync(portfolio, "Portfolio.json");

var bonds = await stockExchange.GetBondsAsync();
var currencies = await stockExchange.GetCurrenciesAsync();
var shares = await stockExchange.GetSharesAsync();

/*foreach (var value in shares)
{
    Console.WriteLine("Name: {0} | Ticker: {1} | PlacementPrice: {2} | Lot: {3}", value.Name, value.Ticker, value.Nominal, value.Lot);
}*/