using Core.Entities.Base;
using Core.Interfaces;

namespace Application.Converters;

public sealed class CurrencyApiClient : CurrencyConvertApiClient
{   
    public CurrencyApiClient(HttpClient httpClient) : base(httpClient)
    {
        httpClient.BaseAddress = new Uri("https://api.currencyapi.com/v3/latest/");
        // httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
    }
}