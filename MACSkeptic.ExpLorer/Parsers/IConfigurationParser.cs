namespace MACSkeptic.ExpLorer.Parsers
{
    public interface IConfigurationParser
    {
        Configuration LoadFrom(string path);
        Configuration LoadFromCurrentAssembly();
    }
}