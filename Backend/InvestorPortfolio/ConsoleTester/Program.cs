﻿using Application.Converters;
using Application.Integration.Tinkoff;
using Core.Entities;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
serviceCollection.AddScoped<ICurrencyConverter<MoneyValue, CurrencyCode>, CurrencyConverter>();
var serviceProvider = serviceCollection.BuildServiceProvider();


// Using
var stockExchange = serviceProvider.GetRequiredService<IStockExchange>();
var converter = serviceProvider.GetRequiredService<ICurrencyConverter<MoneyValue, CurrencyCode>>();

var bonds = await stockExchange.GetBondsAsync();
var currencies = await stockExchange.GetCurrenciesAsync();
var shares = await stockExchange.GetSharesAsync();

var portfolio = new Portfolio();
portfolio.TryAddAccount(title: "TestAccount", out var account);

foreach (var value in shares)
{
    Console.WriteLine("Name: {0} | Ticker: {1} | PlacementPrice: {2} | Lot: {3}", value.Name, value.Ticker, value.Nominal, value.Lot);
}