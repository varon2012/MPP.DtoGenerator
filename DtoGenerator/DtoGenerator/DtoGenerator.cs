using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Formatting;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Options;
using System.Threading;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        private string classesNamespace;
        private int maxThreadsCount;
        private PropertyType propertyType = new PropertyType();
        private ClassDescriptionList classDescriptionList;
        private Dictionary<string, List<StringBuilder>> resultUnits = new Dictionary<string, List<StringBuilder>>();
        private static object locker = new object();        
        private static ManualResetEvent[] resetEvents;
        private int[] eventThreadConnection;
        
        public DtoGenerator(int maxThreadsCount, string classesNamespace)
        {
            this.classesNamespace = classesNamespace;
            this.maxThreadsCount = maxThreadsCount;
            resetEvents = new ManualResetEvent[maxThreadsCount];
            eventThreadConnection = new int[maxThreadsCount];
        }

        public Dictionary<string, List<StringBuilder>> GenerateClasses(string jsonFile)
        {
            classDescriptionList = JsonConvert.DeserializeObject<ClassDescriptionList>(jsonFile);
            for (int i = 0; i < classDescriptionList.classDescriptions.Count(); i++)
            {
                if (i < maxThreadsCount)
                {
                    eventThreadConnection[i] = i;
                    resetEvents[i] = new ManualResetEvent(false);                    
                }
                else
                {
                    int index = WaitHandle.WaitAny(resetEvents);
                    eventThreadConnection[index] = i;
                    resetEvents[index].Reset();
                }

                ThreadPool.QueueUserWorkItem(new WaitCallback(GetAllUnits), i);
            }

            return resultUnits;
        }

        private void GetAllUnits(object index)
        {
            int resetEventIndex = 0;
            while (eventThreadConnection[resetEventIndex] != (int)index) 
                resetEventIndex++;
            ClassDescription classDescription = classDescriptionList.classDescriptions[(int)index];
            lock (locker)
            {
                resultUnits.Add(classDescription.ClassName, new List<StringBuilder>());
                resultUnits[classDescription.ClassName].Add(ConvertUnitToString(GenerateUnit(classDescription)));
                resetEvents[resetEventIndex].Set();
            }
        }

        private CompilationUnitSyntax GenerateUnit(ClassDescription classDescription)
        {
            CompilationUnitSyntax compilationUnit = CompilationUnit();
            compilationUnit = compilationUnit.AddMembers(AddNamespace(classDescription));

            return compilationUnit;    
        }

        private NamespaceDeclarationSyntax AddNamespace(ClassDescription classDescription)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(classesNamespace));
            namespaceDeclaration = namespaceDeclaration.AddMembers(AddClass(classDescription));

            return namespaceDeclaration;
        }

        private ClassDeclarationSyntax AddClass(ClassDescription classDescription)
        {
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(classDescription.ClassName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword));
            classDeclaration = AddProperties(classDeclaration, classDescription.Properties);

            return classDeclaration;
        }

        private ClassDeclarationSyntax AddProperties(ClassDeclarationSyntax classDeclaration, 
            List<Property> properties)
        {
            foreach (var property in properties)
            {
                classDeclaration = classDeclaration.AddMembers(GenerateProperty(property));
            }

            return classDeclaration;
        }

        private PropertyDeclarationSyntax GenerateProperty(Property property)
        {
            string type = propertyType.GetType(property.Type, property.Format);
            if (type == null)
                throw new ArgumentNullException();
            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(ParseTypeName(type), property.Name)
                        .AddModifiers(Token(SyntaxKind.PublicKeyword))
                        .AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
                        .AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
            return propertyDeclaration;
        }

        private StringBuilder ConvertUnitToString (CompilationUnitSyntax unit)
        {
            AdhocWorkspace workspace = new AdhocWorkspace();
            SyntaxNode syntaxNode = Formatter.Format(unit, workspace, SetOptions(workspace));

            return WriteToString(syntaxNode);
        }

        private OptionSet SetOptions (AdhocWorkspace workspace)
        {
            OptionSet options = workspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false);
            options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, false);

            return options;
        }

        private StringBuilder WriteToString(SyntaxNode syntaxNode)
        {
            StringBuilder resultString = new StringBuilder();
            using (StringWriter writer = new StringWriter(resultString))
            {
                syntaxNode.WriteTo(writer);
            }

            return resultString;
        }
    }
}
