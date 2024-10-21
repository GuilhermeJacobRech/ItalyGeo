using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WikiDataExtractor.Services;
using WikiDataExtractor.Manipulators;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Auth;
using Serilog;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
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
            RegionManipulator regionManipulator = new(wikipediaApi, italyGeoApi, logger);
            var regionsHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.RegionsOfItalyPagePath);
            if (regionsHtml == null)
            {
                logger.Error("Could not parse Regions of Italy HTML");
                return;
            }
            var regionsToProcess = await regionManipulator.ParseHtmlAsync(regionsHtml);

            await Parallel.ForEachAsync(regionsToProcess, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, async (region, ct) =>
            {
                if (region is AddRegionRequest addRegionRequest)
                {
                    var response = await italyGeoApi.CreateRegionAsync(addRegionRequest);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error($"INSERT of region {addRegionRequest.Name} resulted in {response.StatusCode.ToString()} - {await response.Content.ReadAsStringAsync()}");
                    }
                }

                if (region is UpdateRegionRequest updateRegionRequest)
                {
                    var response = await italyGeoApi.UpdateRegionAsync(updateRegionRequest.Id, updateRegionRequest);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error($"UPDATE of region {updateRegionRequest.Name} resulted in {response.StatusCode.ToString()} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
            });

            /*ProvinceManipulator provinceManipulator = new(wikipediaApi, italyGeoApi, logger);
            var provincesHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.ProvincesOfItalyPagePath);
            var provincesToProcess = await provinceManipulator.ParseHtmlAsync(provincesHtml);

            await Parallel.ForEachAsync(provincesToProcess, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, async (province, ct) =>
            {
                if (province is AddProvinceRequest addProvinceRequest)
                {
                    var response = await italyGeoApi.CreateProvinceAsync(addProvinceRequest);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error($"INSERT of province {addProvinceRequest.Name} resulted in {response.StatusCode.ToString()} - {await response.Content.ReadAsStringAsync()}");
                    }
                }

                if (province is UpdateProvinceRequest updateProvinceRequest)
                {
                    var response = await italyGeoApi.UpdateProvinceAsync(updateProvinceRequest.Id, updateProvinceRequest);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error($"UPDATE of province {updateProvinceRequest.Name} resulted in {response.StatusCode.ToString()} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
            });*/

            /*ComuneManipulator comuneManipulator = new(wikipediaApi, italyGeoApi, logger);
            var comunesHtml = await wikipediaApi.GetPageHtmlAsync(WikiHelper.ComunesOfItalyPagePath);

            await foreach (var listComunesByLetter in comuneManipulator.ParseHtmlAsync(comunesHtml))
            {
                Console.WriteLine("Insert executando!");
                await Parallel.ForEachAsync(listComunesByLetter, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, async (comuneByLetter, ct) =>
                {
                    var response = await italyGeoApi.CreateComuneAsync(comuneByLetter);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error($"Insert of comune {comuneByLetter.Name} resulted in {response.StatusCode.ToString()} - Msg: {await response.Content.ReadAsStringAsync()}");
                    }
                });
            }*/

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



