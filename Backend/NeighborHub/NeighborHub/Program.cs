using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using NeighborHub.Api;
// ... other usings

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. Services
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        // This converter tells .NET to send strings instead of numbers for Enums
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiServices(builder.Configuration);


WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    IConfiguration configuration = services.GetRequiredService<IConfiguration>();

    await SeedRoleData.SeedRolesAndAdminAsync(services, configuration);
}



// 2. Configure Folders
// Using app.Environment is safer here than builder.Environment
var itemImagesPath = Path.Combine(builder.Environment.WebRootPath, "item-images");
var profileImagesPath = Path.Combine(builder.Environment.WebRootPath, "profile-images");

if (!Directory.Exists(itemImagesPath))
{
    Directory.CreateDirectory(itemImagesPath);
}

if (!Directory.Exists(profileImagesPath))
{
    Directory.CreateDirectory(profileImagesPath);
}

// 3. Middleware Pipeline (Order Matters!)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
// app.UseHttpsRedirection();


app.UseRouting();

// IMPORTANT: Put UseCors BEFORE UseStaticFiles
app.UseCors("AllowFrontend"); 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(itemImagesPath),
    RequestPath = "/item-images",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
    }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(profileImagesPath),
    RequestPath = "/profile-images",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
    }
});


app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
