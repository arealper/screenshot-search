using Microsoft.EntityFrameworkCore;
using ScreenshotSearchApi.DataContext;
using ScreenshotSearchApi.Services;

var builder = WebApplication.CreateBuilder(args);

var dbDir = Path.Combine(AppContext.BaseDirectory, "db");
Directory.CreateDirectory(dbDir);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={Path.Combine(dbDir, "screenshots.db")}"));

builder.Services.AddHttpClient<EmbeddingService>();
builder.Services.AddScoped<OcrService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}

app.UseCors("AllowAngular");


app.UseStaticFiles();
app.MapControllers();

app.Run();
