using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DtoPlugin;

namespace DtoGenerator.Plugins
{
    internal class PluginLoader
    {
        private List<Exception> loadingExceptions;
        private readonly TypeTable typeTable;
        internal PluginLoader()
        {
            typeTable = new TypeTable();
            loadingExceptions = new List<Exception>();
        }

        internal AggregateException LoadingExceptions => new AggregateException("Plugin loading errors", loadingExceptions);
        internal TypeTable TypeTable => typeTable;

        internal void LoadExternalTypes(string pluginsDirectory)
        {
            foreach (string file in Directory.EnumerateFiles(pluginsDirectory))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] exportedTypes = assembly.GetExportedTypes();

                    Type[] plugins = Array.FindAll(exportedTypes,
                        type => typeof(IDtoPlugin).IsAssignableFrom(type) && !type.IsAbstract);

                    CreatePluginsInstaces(plugins);
                }
                catch (Exception ex)
                {
                    loadingExceptions.Add(ex);
                }
            }
        }

        private void CreatePluginsInstaces(Type[] plugins)
        {
            foreach (Type plugin in plugins)
            {
                IDtoPlugin pluginInstance = (IDtoPlugin)Activator.CreateInstance(plugin);
                typeTable.AddType(pluginInstance);
            }
        }

    }
}
