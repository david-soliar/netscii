using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using netscii.Models;
using netscii.Repositories;
using netscii.Services;
using netscii.Services.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddDbContext<NetsciiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<FontRepository>();
builder.Services.AddScoped<ColorRepository>();
builder.Services.AddScoped<ConversionLoggingRepository>();
builder.Services.AddScoped<SuggestionRepository>();

builder.Services.AddScoped<FontService>();
builder.Services.AddScoped<ColorService>();
builder.Services.AddScoped<ConversionService>();
builder.Services.AddScoped<ConversionLoggingService>();
builder.Services.AddScoped<SuggestionService>();
builder.Services.AddScoped<CaptchaService>();

builder.Services.AddScoped<ConversionViewModelFactory>();
builder.Services.AddScoped<SuggestionViewModelFactory>();

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NetsciiContext>();
    await dbContext.Database.MigrateAsync();
    DBInitializer.Initialize(dbContext);
}

app.Run();
