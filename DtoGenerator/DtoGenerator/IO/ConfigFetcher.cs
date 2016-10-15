using System;
using System.Collections.Specialized;

namespace DtoGenerator.IO
{
    internal sealed class ConfigFetcher
    {
        private readonly NameValueCollection _appSettimgs;

        public ConfigFetcher(NameValueCollection appSettimgs)
        {
            if (appSettimgs == null) throw new ArgumentNullException(nameof(appSettimgs));
            _appSettimgs = appSettimgs;
        }

        public string GetStringConfig(string configName, bool isMandatory = true)
        {
            if (configName == null) throw new ArgumentNullException(nameof(configName));
            var config = _appSettimgs[configName];

            if (isMandatory && config == null)
            {
                throw new BadInputException($"Unresolved config {configName}");
            }

            return config;
        }

        public int GetIntConfig(string configName)
        {
            if (configName == null) throw new ArgumentNullException(nameof(configName));
            var config = _appSettimgs[configName];

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
