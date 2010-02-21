using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class LoreConfigurationParser : IConfigurationParser
    {
        public virtual Configuration LoadFrom(string path)
        {
            var lore = Directory.GetFiles(path, "*.lore").First();
            var loreFile = new FileInfo(lore);
            var loreConfiguration = new Configuration(loreFile.Name.Replace(".lore", string.Empty));

            ParseFile(loreFile, loreConfiguration);
            return loreConfiguration;
        }

        public virtual Configuration LoadFromCurrentAssembly()
        {
            return LoadFrom(new FileInfo(Assembly.GetCallingAssembly().Location).Directory.FullName);
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
                var newFile = new FileInfo(Path.Combine(file.Directory.FullName, newName + ".tale"));
                var newConfiguration = new Configuration(newName, string.Empty, configuration);
                ParseFile(newFile, newConfiguration);
            }
        }
    }
}