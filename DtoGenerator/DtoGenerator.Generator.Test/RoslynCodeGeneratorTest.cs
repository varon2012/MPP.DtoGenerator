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

            var expectedResults = new Dictionary<string, string>
            {
                { "SomethingWicked",
@"namespace Test
{
    public sealed class SomethingWicked 
    {
        public System.Int32 FirstProperty { get; set; }
        public System.Int64 SecondProperty { get; set; }
        public System.Single ThirdProperty { get; set; }
    }
}
" },
                { "SomethingBad",
@"namespace Test
{
    public sealed class SomethingBad 
    {
        public System.Int32 MyProperty { get; set; }
        public System.Boolean OnlyMine { get; set; }
    }
}
"
 },
                { "SomethingWicked1",
@"namespace Test
{
    public sealed class SomethingWicked1 
    {
        public System.Int64 SecondProperty { get; set; }
        public System.Single ThirdProperty { get; set; }
    }
}
" },
                { "SomethingWicked2",
@"namespace Test
{
    public sealed class SomethingWicked2 
    {
        public System.Int32 FirstProperty { get; set; }
    }
}
" },
                { "SomethingBad1",
@"namespace Test
{
    public sealed class SomethingBad1 
    {
        public System.Int32 MyProperty { get; set; }
        public System.Boolean OnlyMine { get; set; }
    }
}
" }
            };

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
