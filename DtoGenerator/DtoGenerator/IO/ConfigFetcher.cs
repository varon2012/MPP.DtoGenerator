using System;
using System.Collections.Specialized;

namespace DtoGenerator.IO
{
    internal sealed class ConfigFetcher
    {
        private readonly NameValueCollection _appSettings;

        public ConfigFetcher(NameValueCollection appSettings)
        {
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));
            _appSettings = appSettings;
        }

        public string GetStringConfig(string configName, bool isMandatory = true)
        {
            if (configName == null) throw new ArgumentNullException(nameof(configName));
            var config = _appSettings[configName];

            if (isMandatory && config == null)
            {
                throw new BadInputException($"Unresolved config {configName}");
            }

            return config;
        }

        public int GetIntConfig(string configName)
        {
            if (configName == null) throw new ArgumentNullException(nameof(configName));
            var config = _appSettings[configName];

            try
            {
                return int.Parse(config);
            }
            catch (ArgumentNullException)
            {
                throw new BadInputException($"Unresolved config {configName}");
            }
            catch (FormatException e)
            {
                throw new BadInputException($"Config {configName} has invalid format: {e.Message}");
            }
        }
    }
}
