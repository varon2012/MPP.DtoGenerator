using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace DtoGenerator.CodeGenerators.GeneratedItems
{
    public class GeneratedClassList : IEnumerable<GeneratedClass>
    {
        private List<GeneratedClass> classes;

        public GeneratedClassList()
        {
            classes = new List<GeneratedClass>();
        }

        public void AddClass(GeneratedClass generatedClass)
        {
            if(generatedClass == null) throw new ArgumentNullException(nameof(generatedClass));

            classes.Add(generatedClass);
        }

        public IEnumerator<GeneratedClass> GetEnumerator()
        {
            return classes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void ClearClassList()
        {
            classes.Clear();
        }
    }
}
