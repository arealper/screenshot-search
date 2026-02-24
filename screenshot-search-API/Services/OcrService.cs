using System.Text;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

namespace ScreenshotSearchApi.Services;

public class OcrService
{
    public string ExtractText(string imagePath)
    {
        var text = GetTextFromBitmap(imagePath).Result;
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

    public static async Task<string> GetTextFromBitmap(string filePath)
    {
        StringBuilder text = new();

        await using (var fileStream = File.OpenRead(filePath))
        {
            var bmpDecoder =
                await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());
            var softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

            var ocrEngine = OcrEngine.TryCreateFromLanguage(new Language("en-US"));
            var ocrResult = await ocrEngine.RecognizeAsync(softwareBmp);

            foreach (var line in ocrResult.Lines) text.AppendLine(line.Text);
        }

        return text.ToString();
    }
}
