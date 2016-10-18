namespace DtoGenerator
{
    public sealed class DtoDeclaration
    {
        public DtoDeclaration(string className, string classFullText)
        {
            ClassName = className;
            ClassFullText = classFullText;
        }

        public string ClassName { get; }
        public string ClassFullText { get; }
    }
}