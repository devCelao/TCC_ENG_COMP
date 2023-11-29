using CMS.System.Configuration;
using CMS.System.Data.Context;
using CMS.System.Data.Repositories;
using CMS.System.Services;
using CMS.System.Services.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                                .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile(path: $"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();
builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();


builder.Services.AddDbContext<DispositivoContext>(optionsAction: options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection")));

builder.Services.AddDbContext<MedicaoContext>(optionsAction: options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection")));

builder.Services.Configure<AppSettingsModel>(builder.Configuration);

builder.Services.AddScoped<IDispositivoRepository, DispositivoRepository>();
builder.Services.AddSingleton<MqttService>();
builder.Services.AddHostedService<MqttIntegrationHandler>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
