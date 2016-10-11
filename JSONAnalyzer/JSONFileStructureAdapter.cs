using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOGenerator;

namespace JSONAnalyzer
{
    public class JSONFileStructureAdapter
    {
        public static List<ClassDescription> AdaptToClassDecriptionList(JSONFileStructure jsonFileStructure)
        {
            TypesStorage typesStorage = TypesStorage.Instance();

            List<ClassDescription> classDescriptions = new List<ClassDescription>();

            foreach (ClassStructure classStructure in jsonFileStructure.ClassDescriptions)
            {
                ClassDescription classDescription = new ClassDescription(classStructure.ClassName);

                foreach (PropertyInfo propertyInfo in classStructure.Properties)
                {
                    NonPlatformType propertyNonPlatformType = typesStorage.nonPlatformTypesMappingDictionary[propertyInfo.Type];
                    Type propertyType = typesStorage.GetPlatformType(propertyNonPlatformType, propertyInfo.Format);
                    classDescription.PropertyDescriptions.Add(new PropertyDescription(propertyInfo.Name, propertyType));
                }

                classDescriptions.Add(classDescription);
            }

            return classDescriptions;
        }
    }
}
