public sealed class CurrencyApiClient : ICurrencyConvertApiClient
{   
    public CurrencyApiClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://api.currencyapi.com/v3/latest/");
        // httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
        Client = httpClient;
    }
}