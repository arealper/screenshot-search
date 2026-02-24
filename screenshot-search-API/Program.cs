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

// Add configuration files
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.secret.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{

}

app.UseCors("AllowAngular");


app.UseStaticFiles();
app.MapControllers();

app.Run();
