using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;

namespace DtoGenerator
{
    public class GenerationClassUnit
    {
        internal GenerationClassUnit(ClassDescription classDescription, string namespaceName, TypeTable typeTable)
        {
            ClassDescription = classDescription;
            NamespaceName = namespaceName;
            TypeTable = typeTable;
        }

        internal ClassDescription ClassDescription { get; }
        internal string NamespaceName { get; }
        internal string Code { get; set; }

        internal TypeTable TypeTable { get; }
    }
}
