namespace DtoGenerator.Config
{
    public interface IConfig
    {
        int MaxTaskCount { get; }
        string NamespaceName { get;  }
        string PluginsDirectory { get; }
    }
}
