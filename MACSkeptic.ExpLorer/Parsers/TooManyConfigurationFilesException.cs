using System;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class TooManyConfigurationFilesException : Exception
    {
        public TooManyConfigurationFilesException(params string[] files)
            : base(
                "Found too many configuration files: [#{filesFound}]".ApplyArguments(
                    new {filesFound = files.JoinAsString("; ")}))
        {
        }
    }
}