using DtoGenerator.Contracts.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DtoGenerator.Services.TableWithTypes
{
    internal static class TableWithTypes
    {
        #region Private Methods

        private static List<ITypeDescription> _table = new List<ITypeDescription>();

        #endregion

        #region Ctor

        static TableWithTypes()
        {
            _table.Add(new TypeDescription("integer", "int32", typeof(int)));
            _table.Add(new TypeDescription("integer", "int64", typeof(long)));
            _table.Add(new TypeDescription("string", "byte", typeof(byte)));
            _table.Add(new TypeDescription("boolean", null, typeof(bool)));
            _table.Add(new TypeDescription("string", "date", typeof(DateTime)));
            _table.Add(new TypeDescription("string", "string", typeof(string)));
        }

        #endregion

        #region Public Methods

        internal static void LoadAdditionalTypes(string folderName)
        {
            foreach (string file in Directory.EnumerateFiles(folderName))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                Type[] exportedTypes = assembly.GetExportedTypes();

                Type[] pluginsTypes = Array.FindAll(exportedTypes,
                    (Type type) => typeof(IPlugin).IsAssignableFrom(type) && !type.IsAbstract);
                foreach (Type pluginType in pluginsTypes)
                {
                    IPlugin plugin =
                        (IPlugin)Activator.CreateInstance(pluginType);
                    AddTypesFromPlugin(plugin);
                }
            }
        }

        internal static Type TranslateToDotNetType(string format)
        {
            return _table.Single((ITypeDescription x) => x.Format == format).DotNetType; 
        }

        #endregion

        #region Private Members

        private static void AddTypesFromPlugin(IPlugin plugin)
        {
            foreach(var typeDescription in plugin.GetTypes())
            {
                _table.Add(typeDescription);
            }
        }

        #endregion
    }
}
