using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("FeedFromHtml")]
[assembly: AssemblyDescription("FeedFromHtml")]
[assembly: AssemblyCompany("FeedFromHtml")]
[assembly: AssemblyCopyright("Ruben Silveira")]
[assembly: AssemblyProduct("FeedFromHtml")]
[assembly: Guid("5B13DC7A-3BFD-419B-91A8-591D07E3C87A")]

[assembly: AssemblyVersion("1.23.12.6")]

namespace FeedFromHtml;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddSingleton<IFeedConfigProvider, InCodeFeedConfigProvider>();
        builder.Services.AddScoped<IFeedRetriever, DirectFeedRetriever>();

        builder.Services.AddLogging();
        builder.Services.AddRazorPages();

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

        WebApplication app = builder.Build();

        app.Use(async (context, next) =>
        {
            context.Features.Set<HttpRequestExtensionFeature>(new HttpRequestExtensionFeature(context));
            await next(context);
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.MapRazorPages();

        app.Run();
    }
}