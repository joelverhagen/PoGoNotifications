using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Knapcode.PoGoNotifications.Test
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

        public static TestOptions TestOptions
        {
            get
            {
                var options = new TestOptions();
                _configuration.Value.Bind(options);
                return options;
            }
        }
    }
}
