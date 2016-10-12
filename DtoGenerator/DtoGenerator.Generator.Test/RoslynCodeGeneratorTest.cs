using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DtoGenerator.Generator.Test
{
    [TestClass]
    public sealed class RoslynCodeGeneratorTest
    {
        [TestMethod]
        public void Test()
        {
            var classDescriptions = new List<DtoClassDescription>
            {
                new DtoClassDescription("SomethingWicked", new List<DtoClassProperty>
                {
                    new DtoClassProperty("FirstProperty", "integer", "int32"),
                    new DtoClassProperty("SecondProperty", "integer", "int64"),
                    new DtoClassProperty("ThirdProperty", "number", "float")
                }),
                new DtoClassDescription("SomethingBad", new List<DtoClassProperty>
                {
                    new DtoClassProperty("MyProperty", "integer", "int32"),
                    new DtoClassProperty("OnlyMine", "boolean")
                }),
                new DtoClassDescription("SomethingWicked1", new List<DtoClassProperty>
                {
                    new DtoClassProperty("FirstProperty", "NotExistedType", "int32"),
                    new DtoClassProperty("SecondProperty", "integer", "int64"),
                    new DtoClassProperty("ThirdProperty", "number", "float")
                }),
                new DtoClassDescription("SomethingWicked2", new List<DtoClassProperty>
                {
                    new DtoClassProperty("FirstProperty", "integer", "int32"),
                    new DtoClassProperty("SecondProperty", "integer", "NotExistedFormat"),
                    new DtoClassProperty("ThirdProperty", "number" /* not specified format */)
                }),
                new DtoClassDescription("SomethingBad1", new List<DtoClassProperty>
                {
                    new DtoClassProperty("MyProperty", "integer", "int32"),
                    new DtoClassProperty("OnlyMine", "boolean", "RedundantFormat")
                }),
            };

            var expectedResults = new Dictionary<string, string>();

            foreach (var classDescription in classDescriptions)
            {
                var className = classDescription.ClassName;
                expectedResults[className] = Resource.ResourceManager.GetString(className);
            }

            var config = new Config
            {
                ClassesNamespace = "Test"
            };

            var result = new ClassCodeGenerator(config, new RoslynCodeGenerator()).Generate(classDescriptions);

            foreach (var className in result.Keys)
            {
                Assert.AreEqual(expectedResults[className], result[className]);
            }
        }
    }
}
