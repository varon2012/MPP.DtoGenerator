using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.DtoDescriptor
{
    class ClassDescriptor : IEnumerable<PropertyDescriptor>
    {
        public string ClassName { get; }
        private List<PropertyDescriptor> properties;

        public ClassDescriptor(string className)
        {
            if (className == null)
            {
                throw new ArgumentNullException();
            }

            ClassName = className;
            properties = new List<PropertyDescriptor>();
        }

        public void addProperty(PropertyDescriptor property)
        {
            if(property == null)
            {
                throw new ArgumentNullException();
            }
            properties.Add(property);
        }

        public IEnumerator<PropertyDescriptor> GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
