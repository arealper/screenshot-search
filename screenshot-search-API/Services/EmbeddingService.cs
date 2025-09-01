using System.Text.Json;

namespace ScreenshotSearchApi.Services;

public class EmbeddingService
{
    private readonly HttpClient _http;
    private readonly string accountId;

    public EmbeddingService(HttpClient http, IConfiguration config)
    {
        var apiKey = config["CLOUDFARE_API_KEY"] ?? throw new Exception("HuggingFace API key missing");
        accountId = config["ACCOUNT_ID"] ?? throw new Exception("Cloudfare AccountId missing");
        _http = http;
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        var url = $"https://api.cloudflare.com/client/v4/accounts/{accountId}/ai/run/@cf/baai/bge-small-en-v1.5";
        var payload = new { text };
        var response = await _http.PostAsJsonAsync(url, payload);

        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var vector = doc.RootElement
                        .GetProperty("result")
                        .GetProperty("data")[0]
                        .EnumerateArray()
                        .Select(e => e.GetSingle())
                        .ToArray();

        return vector;
    }
}
