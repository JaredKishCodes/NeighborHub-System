using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NeighborHub.Api;
using NeighborHub.Api.Hubs;
using System.Text.Json.Serialization;
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
// 3. Middleware Pipeline (Order Matters!)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

// app.UseHttpsRedirection();
app.UseRouting();

// 1. CORS MUST GO BEFORE STATIC FILES & AUTH
app.UseCors("AllowFrontend");

// 2. FIXED: Allow your Vercel site to load item images


// for production: "https://neighbor-hub-system-u5qv.vercel.app"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(itemImagesPath),
    RequestPath = "/item-images",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "https://neighbor-hub-system-u5qv.vercel.app");
    }
});

// 3. FIXED: Allow your Vercel site to load profile images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(profileImagesPath),
    RequestPath = "/profile-images",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "https://neighbor-hub-system-u5qv.vercel.app");
    }
});

// 4. FIXED: Added UseAuthentication right before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

// --- AUTOMATIC PRODUCTION MIGRATIONS ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Automatically finds your database context and runs pending migrations
        var context = services.GetRequiredService<NeighborHub.Infrastructure.Persistence.AppDbContext>();

        Console.WriteLine("Checking for pending database migrations...");
        context.Database.Migrate();
        Console.WriteLine("Database is up to date!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database in production.");
    }
}
// ----------------------------------------

app.Run();


