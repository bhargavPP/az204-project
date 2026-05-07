using Azure.Storage.Blobs;
using Delivery.Api.Service;
 
using Azure.Identity;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
if (!builder.Environment.IsDevelopment())
{
    try
    {
        var keyVaultUrl = builder.Configuration["KeyVaultUrl"];

        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUrl!),
            new DefaultAzureCredential());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Key Vault failed: {ex.Message}");
    }
    
}  

builder.Services.AddControllers(); // FIX: Adds support for [ApiController] and Controllers
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Optional: Customize API info (Title, Version, etc.)
    options.SwaggerDoc("v1", new  OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API for testing purposes"
    });
});
// ADD THIS LINE
builder.Services.AddAuthorization();

builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration["StorageConnection"]));
builder.Services.AddScoped<BlobService>();

builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

app.UseSwagger();   // Serves the raw JSON file (swagger.json)
app.UseSwaggerUI(); // Serves the visual HTML page (index.html)
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

 