using System.Text.Json;

public class JsonSaver<T> : IAsyncFileSaver<T>
{
    public T Recovery(string path)
    {
         try 
        {
            var stream = File.OpenRead(path);
            return JsonSerializer.Deserialize<T>(stream)!;
        }
        catch 
        {
            return default(T)!;
        }
    }

    public async Task<T> RecoveryAsync(string path)
    {
        try 
        {
            var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(stream)!;
        }
        catch 
        {
            return default(T)!;
        }
    }

    public bool Save(T objectToSave, string path)
    {
         try 
        {
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, objectToSave, new JsonSerializerOptions() {
                AllowTrailingCommas = true,
                WriteIndented = true,
            });
            return true;
        }
        catch 
        {
            return false;
        }
    }

    public async Task<bool> SaveAsync(T objectToSave, string path)
    {
        try 
        {
            await using FileStream createStream = File.Create(path);
            await JsonSerializer.SerializeAsync(createStream, objectToSave, new JsonSerializerOptions() {
                AllowTrailingCommas = true,
                WriteIndented = true,
            });
            return true;
        }
        catch 
        {
            return false;
        }
    }
}