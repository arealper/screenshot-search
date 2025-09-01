using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ScreenshotSearchApi.Models;

public class Screenshot
{
    public int Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string ExtractedText { get; set; } = string.Empty;
    public string EmbeddingJson { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Helper to deserialize embedding
    [NotMapped]
    public float[] Embedding
    {
        get => string.IsNullOrEmpty(EmbeddingJson)
               ? Array.Empty<float>()
               : JsonSerializer.Deserialize<float[]>(EmbeddingJson)!;
        set => EmbeddingJson = JsonSerializer.Serialize(value);
    }
}
