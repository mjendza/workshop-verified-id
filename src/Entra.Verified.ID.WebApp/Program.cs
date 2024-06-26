using Entra.Verified.ID.WebApp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettingsModel>(builder.Configuration.GetSection("GuardianFaceCheckCard"));

builder.Services.Configure<EntraServicePrincipal>(builder.Configuration.GetSection("EntraTenantServicePrincipal"));


builder.Services.AddHttpContextAccessor();
await builder.Services.AddVcServices();
builder.Services.AddMemoryCache();


builder.Services.AddHttpLogging(options => { options.LoggingFields = HttpLoggingFields.All; });

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();
if (app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseHttpLogging();

app.UseDefaultFiles();
app.UseRouting();
app.UseCors("MyPolicy"); //after routing and before authorization - drama :(
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", $"public, max-age={1}");
    }
});

app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
app.Run();

namespace Entra.Verified.ID.WebApp
{
    public class Program
    {
    }
}