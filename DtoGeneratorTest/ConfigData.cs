using DtoGenerator.Config;
using System.Configuration;

namespace DtoGeneratorTest
{
    public class ConfigData: IConfig
    {
        public int MaxTaskCount => int.Parse(GetConfig("maxTaskCount"));
        public string NamespaceName => GetConfig("namespace");
        public string PluginsDirectory => GetConfig("pluginsDirectory");

        private string GetConfig(string elementName)
        {
            return ConfigurationManager.AppSettings[elementName];
        }
    }
}
