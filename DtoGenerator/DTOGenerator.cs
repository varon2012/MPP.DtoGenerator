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

        private readonly static object locker = new object();

        public struct ThreadContex
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
            ThreadPool.SetMaxThreads(CountOfTasks, Int32.MaxValue);

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
            return result;
        }

        private void ThreadPoolCallback(Object threadContext)
        {
            ThreadContex data = (ThreadContex)threadContext;
            Dictionary<string, CodeCompileUnit> result = (Dictionary<string, CodeCompileUnit>)data.result;
            CodeCompileUnit unit = GenerateCode(data.classDescription, data.Namespace);
            lock (locker)
            {
                result.Add(data.classDescription.className, unit);
            }    
            data.doneEvent.Set();
        }

        private CodeCompileUnit GenerateCode(ClassDescriptor classDescription, String NameSpace)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace classNameSpace = new CodeNamespace(NameSpace);
            compileUnit.Namespaces.Add(classNameSpace);

            CodeTypeDeclaration className = new CodeTypeDeclaration(classDescription.className);
            className.IsClass = true;
            className.TypeAttributes = System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Sealed;
            classNameSpace.Types.Add(className);

            foreach(var property in classDescription.properties)
            {
                CodeMemberProperty tempProperty = new CodeMemberProperty();
                tempProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                tempProperty.Type = new CodeTypeReference(GetType(property.format,property.type));
                tempProperty.Name = property.name;
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
            return "UnknownType";
        }
    }

}
