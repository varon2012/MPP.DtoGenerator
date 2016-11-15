using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;
using System.Configuration;

namespace DTO_Generator
{
    class Configuration : IConfiguration
    {
        public int MaxTaskCount => int.Parse(GetConfig("maxTaskCount"));
        public string Namespace => GetConfig("namespace");

        public string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
