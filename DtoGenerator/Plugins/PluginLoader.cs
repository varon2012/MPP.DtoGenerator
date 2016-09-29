using System;
using System.IO;
using System.Reflection;
using DtoPlugin;

namespace DtoGenerator.Plugins
{
    public class PluginLoader
    {
        private readonly TypeTable typeTable;
        public PluginLoader()
        {
            typeTable = new TypeTable();
        }

        public TypeTable TypeTable => typeTable;

        public void LoadExternalTypes(string pluginsDirectory)
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
                catch
                {
                    throw new InvalidOperationException("Plugin loading exception");
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
