using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Repositories;
using netscii.Services;
using netscii.Services.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<NetsciiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<FontService>();
builder.Services.AddScoped<ColorService>();
builder.Services.AddScoped<ConversionService>();
builder.Services.AddScoped<ConversionLoggingService>();

builder.Services.AddScoped<ConversionViewModelFactory>();

builder.Services.AddScoped<FontRepository>();
builder.Services.AddScoped<ColorRepository>();
builder.Services.AddScoped<ConversionLoggingRepository>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NetsciiContext>();
    DBInitializer.Initialize(dbContext);
}

app.Run();

// examples, log aktivity/historia, suggestions
// abstrahovat priame acces z _layout do util nejako
