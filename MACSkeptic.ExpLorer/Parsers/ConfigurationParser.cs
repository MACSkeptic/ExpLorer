using System.IO;
using System.Linq;
using System.Reflection;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class ConfigurationParser : IConfigurationParser
    {
        private readonly string _mainFileExtension;
        private readonly IFileResolver _fileResolver;
        private readonly string _subFileExtension;

        public ConfigurationParser(IFileResolver fileResolver, string mainFileExtension, string subFileExtension)
        {
            _fileResolver = fileResolver;
            _mainFileExtension = mainFileExtension;
            _subFileExtension = subFileExtension;
        }

        public ConfigurationParser(IFileResolver fileResolver, string mainFileExtension)
            : this(fileResolver, mainFileExtension, mainFileExtension)
        {
        
        }

        public virtual Configuration LoadFromPath(string path)
        {
            var loreFiles = Directory.GetFiles(path, "*." + _mainFileExtension);

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
            var loreFile = _fileResolver.Resolve(path, _mainFileExtension);
            var loreConfiguration = new Configuration(loreFile.Name.Replace("." + _mainFileExtension, string.Empty));

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
                    new FileInfo(Path.Combine(file.Directory.FullName, newName + "." + _subFileExtension));
                var newConfiguration = new Configuration(newName, c => c.BelongingTo(configuration));
                ParseFile(newFile, newConfiguration);
            }
        }
    }
}