using Microsoft.Extensions.FileProviders;
using NeighborHub.Api;
// ... other usings

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiServices(builder.Configuration);

WebApplication app = builder.Build();

// 2. Configure Folders
// Using app.Environment is safer here than builder.Environment
string resourcesPath = Path.Combine(app.Environment.ContentRootPath, "Resources");

if (!Directory.Exists(resourcesPath))
{
    Directory.CreateDirectory(resourcesPath);
}

// 3. Middleware Pipeline (Order Matters!)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

// IMPORTANT: Put UseCors BEFORE UseStaticFiles
app.UseCors("AllowAll"); 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(resourcesPath),
    RequestPath = "/Resources"
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();