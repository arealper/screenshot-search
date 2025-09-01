using Microsoft.AspNetCore.Mvc;
using ScreenshotSearchApi.DataContext;
using ScreenshotSearchApi.Models;
using ScreenshotSearchApi.Services;

namespace ScreenshotSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly AppDbContext _db;    
    private readonly IWebHostEnvironment _env;
    private readonly OcrService _ocrService;
    private readonly EmbeddingService _embeddingService;    

    public UploadController(OcrService ocrService, 
        EmbeddingService embeddingService, 
        AppDbContext db, 
        IWebHostEnvironment environment)
    {
        _ocrService = ocrService;
        _embeddingService = embeddingService;
        _db = db;
        _env = environment;
    }

    [HttpPost]
    [RequestSizeLimit(20_000_000)] // 20MB
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // Save file
        var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/{file.FileName}";

        // Extract text
        var text = _ocrService.ExtractText(filePath);

        // Get embedding
        var embedding = await _embeddingService.GetEmbeddingAsync(text);

        // Save to DB 
        var screenshot = new Screenshot
        {
            FilePath = publicUrl,
            ExtractedText = text,
            Embedding = embedding
        };

        _db.Screenshots.Add(screenshot);
        await _db.SaveChangesAsync();

        return Ok(new { screenshot.Id, screenshot.FilePath, screenshot.ExtractedText });
    }
}
