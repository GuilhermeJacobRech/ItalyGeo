using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WikiDataExtractor.Services;
using WikiDataExtractor.Manipulators;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Auth;
using Serilog;
class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        /*var configuration = new ConfigurationBuilder()
             .AddJsonFile($"servicesettings.json");
        var config = configuration.Build();*/

        var italyGeoApi = host.Services.GetRequiredService<IItalyGeoApi>();
        var wikipediaApi = host.Services.GetRequiredService<IWikipediaApi>();
        var logger = host.Services.GetRequiredService<Serilog.ILogger>();

        Credential credential = new()
        {
            Username = "admin",
            Password = "password"
        };

        if (await italyGeoApi.AuthenticateAsync(credential))
        {
            /*RegionManipulator regionManipulator = new(wikipediaApi, italianCitProTraApi);
            var regionsHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.RegionsOfItalyPagePath);
            var regionsToAdd = await regionManipulator.ParseHtmlAsync(regionsHtml);
            List<Task> TasksRegionsToAdd = [];
            foreach (var region in regionsToAdd) TasksRegionsToAdd.Add(italianCitProTraApi.CreateRegionAsync(region));
            await Task.WhenAll(TasksRegionsToAdd);

            ProvinceManipulator provinceManipulator = new(wikipediaApi, italianCitProTraApi);
            var provincesHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.ProvincesOfItalyPagePath);
            var provincesToAdd = await provinceManipulator.ParseHtmlAsync(provincesHtml);
            List<Task> TasksProvincesToAdd = [];
            foreach (var province in provincesToAdd) TasksProvincesToAdd.Add(italianCitProTraApi.CreateProvinceAsync(province));
            await Task.WhenAll(TasksProvincesToAdd);*/

            ComuneManipulator comuneManipulator = new(wikipediaApi, italyGeoApi, logger);
            var comunesHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.ComunesOfItalyPagePath);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

            var comunesToAdd = await comuneManipulator.ParseHtmlAsync(comunesHtml);
            await Parallel.ForEachAsync(comunesToAdd, options, async (comune, ct) =>
            {
                await italyGeoApi.CreateComuneAsync(comune);
            });

            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.File("Logs/WikiDataExtractor_Log.txt")
            .MinimumLevel.Error()
            .CreateLogger();

        var host = Host.CreateDefaultBuilder(args)
       .ConfigureServices((services) =>
        {
            services.AddHttpClient<IWikipediaApi, WikipediaApi>(client =>
                client.BaseAddress = new Uri("https://it.wikipedia.org/api/rest_v1/"));

            services.AddHttpClient<IItalyGeoApi, ItalyGeoApi>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7171/");
            });

            services.AddSerilog(logger);
        });
        return host;
    }
}



