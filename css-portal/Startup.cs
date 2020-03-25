using Gov.Pssg.Css.Interfaces.DynamicsAutorest;
using Gov.Pssg.Css.Interfaces.SharePoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Net.Http;

namespace Gov.Pssg.Css.Public
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            if (!string.IsNullOrEmpty(_configuration["DYNAMICS_ODATA_URI"]))
            {
                // Add Dynamics
                services.AddTransient(new Func<IServiceProvider, IDynamicsClient>((serviceProvider) =>
                {
                    IDynamicsClient client = DynamicsSetupUtil.SetupDynamics(_configuration);
                    return client;
                }));

            }

            if (!string.IsNullOrEmpty(_configuration["SHAREPOINT_ODATA_URI"]))
            {
                // add SharePoint.

                services.AddTransient<SharePointFileManager>(_ => new SharePointFileManager(_configuration));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string basePath = _configuration["BASE_PATH"];
            if (!string.IsNullOrEmpty(basePath))
            {
                app.UsePathBase(basePath);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            // enable Splunk logger using Serilog
            if (!string.IsNullOrEmpty(_configuration["SPLUNK_COLLECTOR_URL"]) &&
                !string.IsNullOrEmpty(_configuration["SPLUNK_TOKEN"])
                )
            {

                Serilog.Sinks.Splunk.CustomFields fields = new Serilog.Sinks.Splunk.CustomFields();
                if (!string.IsNullOrEmpty (_configuration["SPLUNK_CHANNEL"]))
                {
                    fields.CustomFieldList.Add(new Serilog.Sinks.Splunk.CustomField("channel", _configuration["SPLUNK_CHANNEL"]));
                }
                var splunkUri = new Uri(_configuration["SPLUNK_COLLECTOR_URL"]);
                var upperSplunkHost = splunkUri.Host?.ToUpperInvariant() ?? string.Empty;

                // Fix for bad SSL issues 


                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .WriteTo.EventCollector( splunkHost: _configuration["SPLUNK_COLLECTOR_URL"],
                       sourceType: "manual", eventCollectorToken: _configuration["SPLUNK_TOKEN"], 
                       restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
#pragma warning disable CA2000 // Dispose objects before losing scope
                       messageHandler: new HttpClientHandler()
                       {
                           ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                       }
#pragma warning restore CA2000 // Dispose objects before losing scope
                     )                    
                    .CreateLogger();

                Serilog.Debugging.SelfLog.Enable(Console.Error);

                Log.Logger.Information("CSA Portal Container Started");
                
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .CreateLogger();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
