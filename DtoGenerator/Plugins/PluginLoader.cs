using System;
using System.IO;
using System.Reflection;
using DtoPlugin;

namespace DtoGenerator.Plugins
{
    internal class PluginLoader
    {
        private readonly TypeTable typeTable;
        internal PluginLoader()
        {
            typeTable = new TypeTable();
        }

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
                catch (Exception)
                {
                    throw new InvalidOperationException("Plugin loading error");
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
