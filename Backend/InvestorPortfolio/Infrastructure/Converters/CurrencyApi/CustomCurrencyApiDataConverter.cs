using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities.SpecificData;
using Infrastructure.DTOs;

namespace Infrastructure.Converters.CurrencyApi;

public class CustomCurrencyApiDataConverter : JsonConverter<CurrencyDataRequest>
{
    public override CurrencyDataRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var data = jsonDocument.RootElement.GetProperty("data");
        var currencyData = new Dictionary<CurrencyCode, MoneyValue>();

        foreach (var currencyElement in data.EnumerateObject())
        {
            var code = currencyElement.Name;
            var valueElement = currencyElement.Value.GetProperty("value");
            var value = valueElement.GetDecimal();

            currencyData[new CurrencyCode(code)] = MoneyValue.FromDecimal(value, new CurrencyCode(code));
        }

        return new CurrencyDataRequest { Data = currencyData };
    }

    public override void Write(Utf8JsonWriter writer, CurrencyDataRequest value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
