using System;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class NoConfigurationFileException : Exception
    {
        public NoConfigurationFileException(string path)
            : base("No configuration file was found on the directory: [#{path}]".ApplyArguments(new { path }))
        {
        }
    }
}