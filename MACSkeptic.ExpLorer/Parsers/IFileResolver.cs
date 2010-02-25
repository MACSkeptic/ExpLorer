using System.IO;

namespace MACSkeptic.ExpLorer.Parsers
{
    public interface IFileResolver
    {
        FileInfo Resolve(string path, string extension);
    }
}