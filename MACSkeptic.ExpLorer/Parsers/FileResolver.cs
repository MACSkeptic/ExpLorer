using System.IO;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class FileResolver : IFileResolver
    {
        public FileInfo Resolve(string path, string extension)
        {
            if (path.IsEmpty())
            {
                throw new InvalidPathException("Path must not be empty/null");
            }

            if (!path.EndsWith(extension))
            {
                path = path + "." + extension;
            }

            var mainFile = new FileInfo(path);
            if (mainFile.Exists)
            {
                return mainFile;
            }

            mainFile = new FileInfo(Path.Combine(mainFile.Directory.FullName, mainFile.Name.ToLower()));
            if (mainFile.Exists)
            {
                return mainFile;
            }

            mainFile =
                new FileInfo(
                    Path.Combine(
                        mainFile.Directory.FullName,
                        mainFile.Name.Substring(0, 1).ToLower() + mainFile.Name.Substring(1)));

            if (mainFile.Exists)
            {
                return mainFile;
            }
            mainFile = new FileInfo(path);
            mainFile = new FileInfo(Path.Combine(mainFile.Directory.Parent.FullName, mainFile.Name));

            if (mainFile.Exists)
            {
                return mainFile;
            }
            mainFile = new FileInfo(Path.Combine(mainFile.Directory.FullName, mainFile.Name.ToLower()));
            if (mainFile.Exists)
            {
                return mainFile;
            }
            return new FileInfo(
                Path.Combine(
                    mainFile.Directory.FullName,
                    mainFile.Name.Substring(0, 1).ToLower() + mainFile.Name.Substring(1)));
        }
    }
}