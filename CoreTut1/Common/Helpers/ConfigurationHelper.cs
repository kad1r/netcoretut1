using System;
using Microsoft.Extensions.Configuration;

namespace Common.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static bool IsDevelopment()
        {
            var settings = GetConfig();
            return (settings["Environment"] != null && settings["Environment"].ToLower() == "development") ? true : false;
        }

        public static bool IsStaging()
        {
            var settings = GetConfig();

            return (settings["Environment"] != null && settings["Environment"].ToLower() == "staging") ? true : false;
        }

        public static bool IsProduction()
        {
            var settings = GetConfig();

            return (settings["Environment"] != null && settings["Environment"].ToLower() == "production") ? true : false;
        }

        public static string GetConfig(string config)
        {
            var settings = GetConfig();

            return settings[config] != null ? settings[config] : string.Empty;
        }
    }
}
