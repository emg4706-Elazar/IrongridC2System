

using System.Text.Json;

namespace Producer.Services;

public class DataLoaderService
{
    public List<T> Load<T>(string filepath)
    {
        string json = File.ReadAllText(filepath);

        return JsonSerializer.Deserialize<List<T>>(json)
            ?? [];
    }
}
