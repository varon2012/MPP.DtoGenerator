using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;

namespace DtoGenerator
{
    public class GeneratingClassUnit
    {
        public GeneratingClassUnit(ClassDescription classDescription, string namespaceName, TypeTable typeTable)
        {
            ClassDescription = classDescription;
            NamespaceName = namespaceName;
            TypeTable = typeTable;
        }

        public ClassDescription ClassDescription { get; private set; }
        public string NamespaceName { get; private set; }
        public string Code { get; set; }

        internal TypeTable TypeTable { get; private set; }
    }
}
