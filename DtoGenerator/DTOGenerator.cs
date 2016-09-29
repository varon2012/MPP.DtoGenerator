using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;

using DtoGenerator.Descriptors;
using TypeDescription;

namespace DtoGenerator
{
    public class DtoGenerator : IDisposable
    {
        private DescriptionsOfClass Classes;
        private List<TypeDescriptor> Types;
        private int TaskCount;
        private int WorkingThreads = 0;
        private Queue<ClassDescriptor> ThreadQueue = new Queue<ClassDescriptor>();

        private ManualResetEvent[] doneEvents;
        private List<Exception> SavedException = new List<Exception>();

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
            TaskCount = tasksCount;            
        }

        public void Dispose()
        {
            if (doneEvents != null)
            {
                foreach (var element in doneEvents)
                {
                    element.Dispose();
                }
            }
        }

        public Dictionary<string,CodeCompileUnit> GetUnitsOfDtoClasses()
        {
            Dictionary<string, CodeCompileUnit> result = new Dictionary<string, CodeCompileUnit>();
            doneEvents = new ManualResetEvent[Classes.classDescriptions.Count];
            int i = 0;

            foreach (var dto in Classes.classDescriptions)
            {
                ThreadQueue.Enqueue(dto);
            }

            while(ThreadQueue.Count != 0)
            {               
                lock (locker)
                {
                    if (WorkingThreads < TaskCount)
                    {
                        doneEvents[i] = new ManualResetEvent(false);
                        ClassDescriptor tempClassDescription = ThreadQueue.Dequeue();
                        ThreadContex tempContext = new ThreadContex();
                        tempContext.result = result;
                        tempContext.classDescription = tempClassDescription;
                        tempContext.doneEvent = doneEvents[i];
                        tempContext.Namespace = Classes.Namespace;
                        i++;

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolCallback), tempContext);
                        WorkingThreads++;
                    }
                }
            }

            WaitHandle.WaitAll(doneEvents);

            if(SavedException.Count != 0)
            {
                throw new AggregateException(SavedException);
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
                    SavedException.Add(error);
                }               
            }
            finally
            {
                lock(locker)
                {
                    WorkingThreads--;                 
                }
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
                tempProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), tempProperty.Name)));
                tempProperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), tempProperty.Name), new CodePropertySetValueReferenceExpression()));
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
