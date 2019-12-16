using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Gov.Pssg.Css.Public
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();

                    string splunkCollectorUrl = hostingContext.Configuration["SPLUNK_COLLECTOR_URL"];
                    string splunkToken = hostingContext.Configuration["SPLUNK_TOKEN"];

                    if (!string.IsNullOrEmpty(splunkCollectorUrl) && !string.IsNullOrEmpty(splunkToken))
                    {
                        // enable Splunk logger using Serilog
                        var fields = new Serilog.Sinks.Splunk.CustomFields();
                        if (!string.IsNullOrEmpty(hostingContext.Configuration["SPLUNK_CHANNEL"]))
                        {
                            fields.CustomFieldList.Add(new Serilog.Sinks.Splunk.CustomField("channel", hostingContext.Configuration["SPLUNK_CHANNEL"]));
                        }

                        loggerConfiguration.WriteTo.EventCollector(
                            splunkCollectorUrl,
                            splunkToken,
                            sourceType: "manual",
                            restrictedToMinimumLevel: LogEventLevel.Information,
                            messageHandler: new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                            }
                        );
                    }
                })
                .UseStartup<Startup>();
    }
}
