namespace JsonToDtoDeserializer
{
    class JsonToDtoDeserializer
    {
        static void Main(string[] args)
        {
            DtoGenerator.Services.DtoGeneratror.DtoGenerator DtoGenerator = new DtoGenerator.Services.DtoGeneratror.DtoGenerator();
            DtoGenerator.Generate("E:\\Json.txt",null);
        }
    }
}
