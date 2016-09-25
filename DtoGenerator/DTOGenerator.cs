using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

using DtoGeneratorProgram.Descriptors;
using TypeDescription;

namespace DtoGenerator
{
    public class DTOGenerator
    {
        private DescriptionsOfClass Classes;
        private List<TypeDescription.TypeDescriptor> Types;
        private int CountOfTasks;

        public DTOGenerator(DescriptionsOfClass classes, List<TypeDescription.TypeDescriptor> types,int tasksCount)
        {
            Classes = classes;
            Types = types;
            CountOfTasks = tasksCount; 
        }

        public List<CodeCompileUnit> GetUnitsOfDtoClasses()
        {

            return new List<CodeCompileUnit>();
        }
        private CodeCompileUnit GenerateCode(ClassDescriptor classDescription)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeTypeDeclaration className = new CodeTypeDeclaration(classDescription.className);
            // Add the new type to the namespace type collection.
            
            

            return new CodeCompileUnit();
        }
    }
}
