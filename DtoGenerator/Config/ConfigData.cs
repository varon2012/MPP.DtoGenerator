using System.Configuration;

namespace DtoGenerator.Config
{
    public class ConfigData
    {
        public int ThreadCount => int.Parse(GetConfig("threadCount"));
        public string NamespaceName => GetConfig("namespace");
        public string PluginsDirectory => GetConfig("pluginsDirectory");

        private string GetConfig(string elementName)
        {
            return ConfigurationManager.AppSettings[elementName];
        }
    }
}
