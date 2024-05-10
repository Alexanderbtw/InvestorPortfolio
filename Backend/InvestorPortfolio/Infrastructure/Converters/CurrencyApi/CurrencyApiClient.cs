using Core.Entities.Base;

namespace Infrastructure.Converters.CurrencyApi;

public sealed class CurrencyApiClient : CurrencyConvertApiClient
{   
    public CurrencyApiClient(HttpClient httpClient) : base(httpClient)
    {
        httpClient.BaseAddress = new Uri("https://api.currencyapi.com/");
        // httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
    }
}