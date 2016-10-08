using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace DtoGenerator.CodeGenerators
{
    public class GeneratedClasses : IEnumerable<CodeCompileUnit>
    {
        private List<CodeCompileUnit> classes;

        public GeneratedClasses()
        {
            classes = new List<CodeCompileUnit>();
        }

        public void AddClass(CodeCompileUnit generatedClass)
        {
            if(generatedClass == null)
            {
                throw new ArgumentNullException(nameof(generatedClass));
            }

            classes.Add(generatedClass);
        }

        public IEnumerator<CodeCompileUnit> GetEnumerator()
        {
            return classes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
