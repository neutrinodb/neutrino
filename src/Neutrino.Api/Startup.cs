using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neutrino.Data;

namespace Neutrino.Api {
    public class Startup {

        public static string DataSetsPath = "DataSets";

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (Configuration["DefaultTimeSerie:Enabled"] == "true") {
                var start = ParseIsoDate("DefaultTimeSerie:Start");
                var end = ParseIsoDate("DefaultTimeSerie:End");
                var interval = Convert.ToInt32(Configuration["DefaultTimeSerie:IntervalInMillis"]);
                var autoExtendStep = Convert.ToInt32(Configuration["DefaultTimeSerie:AutoExtendStep"]);
                TimeSerieService.DefaultTimeSerieHeader = new TimeSerieHeader("", start, end, interval, OccurrenceKind.Decimal, autoExtendStep);
            }
            if (!String.IsNullOrEmpty(Configuration["DataFiles:Path"])) {
                DataSetsPath = Configuration["DataFiles:Path"];
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<ITimeSerieService>(s => new TimeSerieService(new FileFinder(DataSetsPath), new FileStreamOpener()));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseStaticFiles();
            app.UseMvc();
        }

        private DateTime ParseIsoDate(string configKey) {
            return DateTime.Parse(Configuration[configKey], null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
    }
}