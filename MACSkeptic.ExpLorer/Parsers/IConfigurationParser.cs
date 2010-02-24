namespace MACSkeptic.ExpLorer.Parsers
{
    public interface IConfigurationParser
    {
        Configuration LoadFromPath(string path);
        Configuration LoadFromFile(string path);
        Configuration LoadFromCurrentAssembly();
    }
}