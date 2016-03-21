using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neutrino.Data;
using System;

namespace Neutrino {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddTransient<ITimeSerieService>(s => new TimeSerieService(new FileFinder("DataSets")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();

            if (Configuration["DefaultTimeSerie:Enabled"] == "true") {
                var start = ParseIsoDate("DefaultTimeSerie:Start");
                var end = ParseIsoDate("DefaultTimeSerie:End");
                var interval = Convert.ToInt32(Configuration["DefaultTimeSerie:IntervalInMillis"]);
                TimeSerieService.DefaultTimeSerie = new TimeSerieHeader("", start, end, interval);
            }
        }

        private DateTime ParseIsoDate(string configKey) {
            return DateTime.Parse(Configuration[configKey], null, System.Globalization.DateTimeStyles.RoundtripKind);
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
