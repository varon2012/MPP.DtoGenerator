
using System;
using System.Configuration;

namespace DtoGeneratorTest
{
    class ApplicationSettingsReader
    {
        private const string GeneratedClassesNamespaceSetting = "generatedClassesNamespace";
        private const string MaxTheadNumberSetting = "maxThreadNumber";

        public string GetClassesNamespace()
        {
            return ConfigurationManager.AppSettings[GeneratedClassesNamespaceSetting]; 
        }
        public int GetMaxThreadNumber()
        {
            return Int32.Parse(ConfigurationManager.AppSettings[MaxTheadNumberSetting]);
        }
    }
}
