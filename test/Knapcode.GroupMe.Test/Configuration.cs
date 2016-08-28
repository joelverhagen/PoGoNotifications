using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Knapcode.GroupMe.Test
{
    public static class Configuration
    {
        private static Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(() =>
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.secret.json", optional: true);

            var configurationRoot = builder.Build();

            return configurationRoot;
        });

        public static string AccessToken => _configuration.Value["AccessToken"];

        public static string BotId => _configuration.Value["BotId"];

        public static string GroupId => _configuration.Value["GroupId"];

        public static string BotName => _configuration.Value["BotName"];
    }
}
