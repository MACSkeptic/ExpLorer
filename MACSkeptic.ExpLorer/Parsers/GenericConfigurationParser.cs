using System.IO;
using System.Linq;
using System.Reflection;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public abstract class GenericConfigurationParser : IConfigurationParser
    {
        protected abstract string MainConfigurationExtension { get; }
        protected abstract string UnderlingConfigurationExtension { get; }

        public virtual Configuration LoadFromPath(string path)
        {
            var loreFiles = Directory.GetFiles(path, "*" + MainConfigurationExtension);

            if (loreFiles.IsEmpty())
            {
                throw new NoConfigurationFileException(path);
            }
            if (loreFiles.Count() > 1)
            {
                throw new TooManyConfigurationFilesException(loreFiles);
            }

            return LoadFromFile(loreFiles.First());
        }

        public virtual Configuration LoadFromFile(string path)
        {
            if (path.IsEmpty())
            {
                throw new InvalidPathException("Path must not be empty/null");
            }
            if (!path.EndsWith(MainConfigurationExtension))
            {
                path = path + MainConfigurationExtension;
            }

            var loreFile = new FileInfo(path);
            if (!loreFile.Exists)
            {
                loreFile = new FileInfo(Path.Combine(loreFile.Directory.FullName, loreFile.Name.ToLower()));
                if (!loreFile.Exists)
                {
                    loreFile =
                        new FileInfo(
                            Path.Combine(
                                loreFile.Directory.FullName,
                                loreFile.Name.Substring(0, 1).ToLower() + loreFile.Name.Substring(1)));

                    if (!loreFile.Exists)
                    {
                        loreFile = new FileInfo(path);
                        loreFile = new FileInfo(Path.Combine(loreFile.Directory.Parent.FullName, loreFile.Name));

                        if (!loreFile.Exists)
                        {
                            loreFile = new FileInfo(Path.Combine(loreFile.Directory.FullName, loreFile.Name.ToLower()));
                            if (!loreFile.Exists)
                            {
                                loreFile =
                                    new FileInfo(
                                        Path.Combine(
                                            loreFile.Directory.FullName,
                                            loreFile.Name.Substring(0, 1).ToLower() + loreFile.Name.Substring(1)));
                            }
                        }
                    }
                }
            }
            var loreConfiguration = new Configuration(loreFile.Name.Replace(MainConfigurationExtension, string.Empty));

            ParseFile(loreFile, loreConfiguration);
            return loreConfiguration;
        }

        public virtual Configuration LoadFromCurrentAssembly()
        {
            return LoadFromPath(new FileInfo(Assembly.GetCallingAssembly().Location).Directory.FullName);
        }

        private void ParseFile(FileInfo file, Configuration configuration)
        {
            var lines = File.ReadAllLines(file.FullName);
            foreach (var line in lines)
            {
                if (line.IsEmpty())
                {
                    continue;
                }

                if (line.Contains(":"))
                {
                    var splat = line.Split(':');
                    var name = splat.First().Trim();
                    var value = splat.Last().Trim();
                    configuration.Add(new Configuration(name, value));
                    continue;
                }

                var newName = line.Trim();
                var newFile =
                    new FileInfo(Path.Combine(file.Directory.FullName, newName + UnderlingConfigurationExtension));
                var newConfiguration = new Configuration(newName, c => c.BelongingTo(configuration));
                ParseFile(newFile, newConfiguration);
            }
        }
    }
}