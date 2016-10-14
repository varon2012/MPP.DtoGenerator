using System;
using System.Configuration;

namespace DTOGenerator
{
    class SettingsManager
    {
        public static int GetThreadPoolLimit()
        {
            int threadPoolLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("threadPoolLimit"), out threadPoolLimit))
            {
                throw new InvalidOperationException("threadPoolLimit is not found in app.config");
            }
            return threadPoolLimit;
        }

        public static string GetDtoNamespace()
        {
            string dtoNamespace = ConfigurationManager.AppSettings.Get("dtoNamespace");
            if ((dtoNamespace == null) || (dtoNamespace == string.Empty))
            {
                throw new InvalidOperationException("dtoNamespace is not found in app.config");
            }
            return dtoNamespace;
        }

    }
}
