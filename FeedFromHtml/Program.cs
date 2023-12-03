using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("FeedFromHtml")]
[assembly: AssemblyDescription("FeedFromHtml")]
[assembly: AssemblyCompany("FeedFromHtml")]
[assembly: AssemblyCopyright("Ruben Silveira")]
[assembly: AssemblyProduct("FeedFromHtml")]
[assembly: Guid("5B13DC7A-3BFD-419B-91A8-591D07E3C87A")]

[assembly: AssemblyVersion("1.23.12.2")]

namespace FeedFromHtml;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IFeedConfigProvider, InCodeFeedConfigProvider>();
        builder.Services.AddTransient<FeedRequestHandler>();
        builder.Services.AddTransient<IFeedRetriever, DirectFeedRetriever>();
        builder.Services.AddLogging();

        builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.Map("/feed/{feedId:alpha:maxlength(64)}", (HttpContext context) =>
        {
            app.Services.GetService<FeedRequestHandler>()?.Handle(context);
        });

        app.Run();
    }
}