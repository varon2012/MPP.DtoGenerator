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
                new DtoClassDescription("SomethingWicked", new List<DtoClassProperty>
                {
                    new DtoClassProperty("FirstProperty", "NotExistedType", "int32"),
                    new DtoClassProperty("SecondProperty", "integer", "int64"),
                    new DtoClassProperty("ThirdProperty", "number", "float")
                }),
                new DtoClassDescription("SomethingWicked", new List<DtoClassProperty>
                {
                    new DtoClassProperty("FirstProperty", "integer", "int32"),
                    new DtoClassProperty("SecondProperty", "integer", "NotExistedFormat"),
                    new DtoClassProperty("ThirdProperty", "number" /* not specified format */)
                }),
                new DtoClassDescription("SomethingBad", new List<DtoClassProperty>
                {
                    new DtoClassProperty("MyProperty", "integer", "int32"),
                    new DtoClassProperty("OnlyMine", "boolean", "RedundantFormat")
                }),
            };

            var expectedResults = new List<string>
            {
@"namespace Test
{
    public sealed class SomethingWicked 
    {
        public System.Int32 FirstProperty { get; set; }
        public System.Int64 SecondProperty { get; set; }
        public System.Single ThirdProperty { get; set; }
    }
}
",
@"namespace Test
{
    public sealed class SomethingBad 
    {
        public System.Int32 MyProperty { get; set; }
        public System.Boolean OnlyMine { get; set; }
    }
}
",
@"namespace Test
{
    public sealed class SomethingWicked 
    {
        public System.Int64 SecondProperty { get; set; }
        public System.Single ThirdProperty { get; set; }
    }
}
",
@"namespace Test
{
    public sealed class SomethingWicked 
    {
        public System.Int32 FirstProperty { get; set; }
    }
}
",
@"namespace Test
{
    public sealed class SomethingBad 
    {
        public System.Int32 MyProperty { get; set; }
        public System.Boolean OnlyMine { get; set; }
    }
}
"
            };

            using (var iterator = expectedResults.GetEnumerator())
            {
                foreach (var classString in new ClassCodeGenerator("Test", new RoslynCodeGenerator()).Generate(classDescriptions))
                {
                    iterator.MoveNext();
                    Assert.AreEqual(iterator.Current, classString);
                }
            }
        }
    }
}
