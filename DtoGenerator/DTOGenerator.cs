using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;

using DtoGenerator.Descriptors;
using TypeDescription;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        private DescriptionsOfClass Classes;
        private List<TypeDescriptor> Types;
        private int CountOfTasks;

        Exception SavedException = null;

        private readonly static object locker = new object();

        private class ThreadContex
        {
            public Dictionary<string, CodeCompileUnit> result;
            public ClassDescriptor classDescription;
            public ManualResetEvent doneEvent;
            public string Namespace;
        }

        public DtoGenerator(DescriptionsOfClass classes, List<TypeDescriptor> types,int tasksCount)
        {
            Classes = classes;
            Types = types;
            CountOfTasks = tasksCount;
        }

        public Dictionary<string,CodeCompileUnit> GetUnitsOfDtoClasses()
        {
            Dictionary<string, CodeCompileUnit> result = new Dictionary<string, CodeCompileUnit>();
            ManualResetEvent[] doneEvents = new ManualResetEvent[Classes.classDescriptions.Count];
            int i = 0;

            foreach (var dtoClass in Classes.classDescriptions)
            {
                doneEvents[i] = new ManualResetEvent(false);

                ThreadContex tempContext = new ThreadContex();
                tempContext.result = result;
                tempContext.classDescription = dtoClass;
                tempContext.doneEvent = doneEvents[i];
                tempContext.Namespace = Classes.Namespace;

                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolCallback),tempContext);
                i++;
            }

            WaitHandle.WaitAll(doneEvents);

            if (SavedException != null)
            {
                throw SavedException;
            }

            return result;
        }

        private void ThreadPoolCallback(Object threadContext)
        {
            ThreadContex data = (ThreadContex)threadContext;
            Dictionary<string, CodeCompileUnit> result = (Dictionary<string, CodeCompileUnit>)data.result;
            CodeCompileUnit unit;
            try
            {
                unit = GenerateCode(data.classDescription, data.Namespace);
                lock (locker)
                {
                    result.Add(data.classDescription.ClassName, unit);
                }
               
            }
            catch(Exception error)
            {
                lock (locker)
                {
                    SavedException = error;
                }               
            }
            finally
            {
                data.doneEvent.Set();
            }
        }

        private CodeCompileUnit GenerateCode(ClassDescriptor classDescription, String NameSpace)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace classNameSpace = new CodeNamespace(NameSpace);
            compileUnit.Namespaces.Add(classNameSpace);

            CodeTypeDeclaration className = new CodeTypeDeclaration(classDescription.ClassName);
            className.IsClass = true;
            className.TypeAttributes = System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Sealed;
            classNameSpace.Types.Add(className);

            foreach(var property in classDescription.Properties)
            {
                CodeMemberProperty tempProperty = new CodeMemberProperty();
                tempProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                string propertyType = GetType(property.Format, property.Type);
                if(propertyType == String.Empty)
                {
                    throw new Exception("Unknown type in class " + classDescription.ClassName + " in property " + property.Name);
                }
                tempProperty.Type = new CodeTypeReference(GetType(property.Format,property.Type));
                tempProperty.Name = property.Name;
                tempProperty.HasGet = true;
                tempProperty.HasSet = true;
                className.Members.Add(tempProperty);
            }
            return compileUnit;
        }

        private string GetType(string format, string type)
        {
            foreach(var description in Types)
            {
                if((description.Format == format)&&(description.Type == type))
                {
                    return description.NETType;
                }
            }
            return String.Empty;
        }
    }

}
