using System;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer
{
    public class InvalidOrMissingConfigurationException : Exception
    {
        public InvalidOrMissingConfigurationException(Configuration root, string name)
            : base(ComposeMessage(root, name))
        {
        }

        private static string ComposeMessage(Configuration root, string name)
        {
            return ("The configuration named [#{configuration}] was requested, " +
                    "but could not be located, on the base configuration [#{full}]. " +
                    "Are you missing a key on your configuration file(s)?").ApplyArguments(
                new {full = root.FullName, configuration = name});
        }
    }
}