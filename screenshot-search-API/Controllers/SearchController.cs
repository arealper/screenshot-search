using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenshotSearchApi.DataContext;
using ScreenshotSearchApi.Services;

namespace ScreenshotSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly EmbeddingService _embeddingService;

    public SearchController(AppDbContext db, EmbeddingService embeddingService)
    {
        _dbContext = db;
        _embeddingService = embeddingService;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        var queryEmbedding = await _embeddingService.GetEmbeddingAsync(query);

        var screenshots = await _dbContext.Screenshots.ToListAsync();

        var results = screenshots
            .Select(s => new
            {
                s.Id,
                s.FilePath,
                s.ExtractedText,
                Score = CosineSimilarity(queryEmbedding, s.Embedding)
            })
            .OrderByDescending(r => r.Score)
            .Take(10);

        return Ok(results);
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}
