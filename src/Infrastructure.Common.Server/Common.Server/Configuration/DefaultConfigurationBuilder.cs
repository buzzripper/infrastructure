using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ProData.Infrastructure.Common.Server.Configuration
{
    public static class DefaultConfigurationBuilder
    {
        private static IConfiguration _configuration;

        public static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
                _configuration = BuildConfiguration();
            return _configuration;
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
                    optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
