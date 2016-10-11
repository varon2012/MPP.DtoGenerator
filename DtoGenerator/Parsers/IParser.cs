namespace DtoGenerator.Parser
{
    internal interface IParser<T>
    {
        T Parse(string jsonString);
    }
}
