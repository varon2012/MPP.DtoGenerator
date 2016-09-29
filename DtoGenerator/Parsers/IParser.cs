using DtoGenerator.DeserializedData;

namespace DtoGenerator.Parsers
{
    public interface IParser
    {
        ClassList ParseClassList(string classDescriptions);
    }
}
