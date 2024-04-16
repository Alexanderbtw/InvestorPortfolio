namespace Core.Entities.Base;

public abstract class CurrencyConvertApiClient
{
    protected CurrencyConvertApiClient(HttpClient client)
    {
        Client = client;
    }

    public HttpClient Client { get; protected set; }
}