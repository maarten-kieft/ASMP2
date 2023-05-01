using Asmp2.Server.Persistence.Extensions;
using Asmp2.Server.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPersistance(Configuration);
        services.AddSignalR();
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddResponseCompression(opts =>
         {
             opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
         });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseResponseCompression();
        app.UseDeveloperExceptionPage();
        app.UseWebAssemblyDebugging();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapHub<MeasurementHub>("/measurementhub");
            endpoints.MapFallbackToFile("index.html");
            endpoints.MapGet("/hoi", async context =>
            {
                await context.Response.WriteAsync("hello");
            });
        });
    }
}
