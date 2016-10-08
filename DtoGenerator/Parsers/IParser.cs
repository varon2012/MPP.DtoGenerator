namespace DtoGenerator.Parser
{
    interface IParser<T>
    {
        T Parse(string jsonString);
    }
}
