using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Text;

using TypeDescription;

namespace DtoGeneratorProgram
{
    internal class FinderTypes
    {
        [ImportMany(typeof(TypeDescriptor))]
        private List<TypeDescriptor> Plugins { get; set; }

        DirectoryCatalog pluginsDirectory;

        public FinderTypes(string path)
        {
            pluginsDirectory = new DirectoryCatalog(path);
        }

        public List<TypeDescriptor> FindPlugins()
        {
            CompositionContainer container = new CompositionContainer(pluginsDirectory);
            container.ComposeParts(this);

            if (Plugins == null)
            {
                return new List<TypeDescriptor>();
            }
            else
            {
                return Plugins;
            }

        }
    }
}
