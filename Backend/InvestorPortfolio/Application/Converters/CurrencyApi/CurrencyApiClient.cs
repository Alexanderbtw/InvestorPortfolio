using Core.Entities.Base;

namespace Application.Converters.CurrencyApi;

public sealed class CurrencyApiClient : CurrencyConvertApiClient
{   
    public CurrencyApiClient(HttpClient httpClient) : base(httpClient)
    {
        httpClient.BaseAddress = new Uri("https://api.currencyapi.com/");
        // httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
    }
}