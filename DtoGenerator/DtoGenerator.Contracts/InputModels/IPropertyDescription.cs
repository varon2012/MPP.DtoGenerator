namespace DtoGenerator.Contracts.InputModels
{
    public interface IPropertyDescription
    {
        string Name { get; set; }
        string Type { get; set; }
        string Format { get; set; }
    }
}
