using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DtoGenerator.Generator;
using DtoGenerator.IO;
using TextFormatters;

namespace DtoGenerator
{
    internal sealed class App
    {
        private const string ClassesNamespace = "classesNamespace";
        private const string MaxTaskCount = "maxTaskCount";
        private const string PluginsDirectoryPath = "pluginsDirectoryPath";
        private const string CodeGenerator = "codeGenerator";
        private const string Logger = "logger";

        private readonly IFileParser<DtoClassDescription> _parser;
        private readonly string _filename;
        private readonly string _outputPath;
        private readonly ConfigFetcher _configFetcher;

        private readonly Dictionary<string, Type> _codeGenerators = new Dictionary<string, Type>
        {
            ["RoslynCodeGenerator"] = typeof(RoslynCodeGenerator)
        };

        private readonly Dictionary<string, Type> _loggers = new Dictionary<string, Type>
        {
            ["ConsoleLogger"] = typeof(ConsoleLogger)
        };

        public App(IFileParser<DtoClassDescription> parser, string filename, string outputPath)
        {
            _parser = parser;
            _filename = filename;
            _outputPath = outputPath;
            _configFetcher = new ConfigFetcher(ConfigurationManager.AppSettings);
        }

        public void Process()
        {
            var classes = _parser.Parse(_filename);

            var generator = new ClassCodeGenerator(Config, GetCodeGenerator());
            var generatedClasses = generator.Generate(classes, GetLogger);

            IClassWriter writter = new FileWriter(_outputPath);

            foreach (var className in generatedClasses.Keys)
            {
                try
                {
                    var classCode = generatedClasses[className];
                    writter.Write(className, classCode);
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine($"Error occurred during writing to file: {e.Message}");
                }
            }
        }

        private Config Config => new Config
        {
            ClassesNamespace = _configFetcher.GetStringConfig(ClassesNamespace),
            MaxTaskCount = _configFetcher.GetIntConfig(MaxTaskCount),
            PluginsDirectoryPath = _configFetcher.GetStringConfig(PluginsDirectoryPath, false)
        };

        private ICodeGenerator GetCodeGenerator()
        {
            var codeGeneratorType = _configFetcher.GetStringConfig(CodeGenerator);
            object[] args =
            {
                GetLogger
            };
            return (ICodeGenerator)Activator.CreateInstance(_codeGenerators[codeGeneratorType], args);
        }

        private ILogger GetLogger => 
            (ILogger)Activator.CreateInstance(_loggers[_configFetcher.GetStringConfig(Logger, false)]);
    }
}
