using Tesseract;

namespace ScreenshotSearchApi.Services;

public class OcrService
{
    public string ExtractText(string imagePath)
    {
        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);
        var text = page.GetText();
        return CleanOcrText(text);
    }

    public static string CleanOcrText(string rawText)
    {
        // 1. Normalize spaces and line breaks
        var cleaned = rawText.Replace("\r", " ")
                             .Replace("\n", " ");

        // 2. Remove weird symbols (keep letters, numbers, ., ,)
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"[^a-zA-Z0-9\s.,]", " ");

        // 3. Collapse multiple spaces
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");

        // 4. Lowercase
        return cleaned.Trim().ToLowerInvariant();
    }
}
