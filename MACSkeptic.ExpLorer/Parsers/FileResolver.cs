using System;
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

            var dotExtension = "." + extension;
            if (!path.EndsWith(extension))
            {
                path = path + dotExtension;
            }

            var mainFile = new FileInfo(path);
            var possibleFiles = new[]
                                    {
                                        path,
                                        Path.Combine(mainFile.Directory.FullName, mainFile.Name.Substring(0, Math.Abs(mainFile.Name.IndexOf("Configuration"))) + dotExtension),
                                        Path.Combine(mainFile.Directory.Parent.FullName, mainFile.Name),
                                        Path.Combine(mainFile.Directory.Parent.FullName, mainFile.Name.Substring(0, Math.Abs(mainFile.Name.IndexOf("Configuration"))) + dotExtension)
                                    };

            foreach (var file in possibleFiles)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                {
                    return fileInfo;
                }
            }

            throw new NoConfigurationFileException(path);
        }
    }
}