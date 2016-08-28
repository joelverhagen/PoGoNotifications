using System.Net;
using System.Net.Http;
using Knapcode.GroupMe;
using Knapcode.PoGoNotifications.Logic;
using Knapcode.PoGoNotifications.Models;
using Knapcode.PoGoNotifications.Models.Pokedex;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Knapcode.PoGoNotifications
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.secret.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<NotificationOptions>(Configuration);
            
            services.AddDbContext<pokedexContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PokedexContext")));
            services.AddDbContext<NotificationContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NotificationContext")));

            {
                var httpClientHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                var httpClient = new HttpClient(httpClientHandler);
                services.AddSingleton(httpClient);
            }

            services.AddTransient<IImageService, ImageService>(x =>
            {
                var options = x.GetService<IOptions<NotificationOptions>>();
                var accessToken = options.Value.GroupMeOptions.AccessToken;
                var httpClient = x.GetService<HttpClient>();
                return new ImageService(accessToken, httpClient);
            });

            services.AddTransient<IBotService, BotService>(x =>
            {
                var options = x.GetService<IOptions<NotificationOptions>>();
                var accessToken = options.Value.GroupMeOptions.AccessToken;
                var httpClient = x.GetService<HttpClient>();
                return new BotService(accessToken, httpClient);
            });
            
            services.AddTransient<INotificationService, GroupMeNotificationService>();
            services.AddTransient<IIgnoredPokemonService, IgnoredPokemonService>();
            services.AddTransient<INotificationBuilder, NotificationBuilder>();
            services.AddTransient<IPokemonEncounterService, PokemonEncounterService>();
            services.AddSingleton<ICheckRepublicService, CheckRepublicService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
