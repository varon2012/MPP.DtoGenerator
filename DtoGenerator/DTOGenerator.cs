using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Threading;

using DtoGenerator.Descriptors;
using TypeDescription;

namespace DtoGenerator
{
    public class DTOGenerator
    {
        private DescriptionsOfClass Classes;
        private List<TypeDescription.TypeDescriptor> Types;
        private int CountOfTasks;

        public struct ThreadConrtex
        {
            public Dictionary<string, CodeCompileUnit> result;
            public ClassDescriptor classDescription;
        }

        public DTOGenerator(DescriptionsOfClass classes, List<TypeDescription.TypeDescriptor> types,int tasksCount)
        {
            Classes = classes;
            Types = types;
            CountOfTasks = tasksCount; 
        }

        public Dictionary<string,CodeCompileUnit> GetUnitsOfDtoClasses()
        {
            Dictionary<string, CodeCompileUnit> result = new Dictionary<string, CodeCompileUnit>();
            ThreadPool.SetMaxThreads(CountOfTasks, Int32.MaxValue);
            foreach (var dtoClass in Classes.classDescriptions)
            {

                //ThreadPool.QueueUserWorkItem
                ThreadConrtex tempContext = new ThreadConrtex();
                tempContext.result = result;
                tempContext.classDescription = dtoClass;
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolCallback),new ThreadConrtex());
            }

            return result;
        }

        private void ThreadPoolCallback(Object threadContext)
        {
            ThreadConrtex data = (ThreadConrtex)threadContext;
            Dictionary<string, CodeCompileUnit> result = (Dictionary<string, CodeCompileUnit>)data.result;
            result.Add(data.classDescription.className, GenerateCode(data.classDescription));
            
        }
        private CodeCompileUnit GenerateCode(ClassDescriptor classDescription)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare a new namespace called Samples.
            CodeNamespace classNameSpace = new CodeNamespace(classDescription.className+"Namespace");
            // Add the new namespace to the compile unit.
            compileUnit.Namespaces.Add(classNameSpace);

            CodeTypeDeclaration className = new CodeTypeDeclaration(classDescription.className);
            className.IsClass = true;
            className.TypeAttributes = System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Sealed;
            classNameSpace.Types.Add(className);

            foreach(var property in classDescription.properties)
            {
                CodeMemberProperty tempProperty = new CodeMemberProperty();
                tempProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                tempProperty.Type = new CodeTypeReference("String");
                tempProperty.Name = property.name;
                tempProperty.HasGet = true;
                tempProperty.HasSet = true;
                className.Members.Add(tempProperty);
            }


            return compileUnit;
        }
    }

}
