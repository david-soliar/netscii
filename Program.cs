using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using netscii.Models;
using netscii.Services;
using netscii.Services.Factories;
using netscii.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSingleton<IHTMLConversionService, HTMLConversionService>();
builder.Services.AddSingleton<ISVGConversionService, SVGConversionService>();
builder.Services.AddSingleton<IANSIConversionService, ANSIConversionService>();
builder.Services.AddSingleton<ILATEXConversionService, LATEXConversionService>();
builder.Services.AddSingleton<IRTFConversionService, RTFConversionService>();
builder.Services.AddSingleton<IEMOJIConversionService, EMOJIConversionService>();
builder.Services.AddSingleton<ITXTConversionService, TXTConversionService>();

builder.Services.AddDbContext<NetsciiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConversionViewModelFactory, ConversionViewModelFactory>();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NetsciiContext>();
    DBInitializer.Initialize(dbContext);
}

app.Run();
