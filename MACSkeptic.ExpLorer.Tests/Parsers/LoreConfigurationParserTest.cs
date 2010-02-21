using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests.Parsers
{
    [TestClass]
    public class LoreConfigurationParserTest
    {
        [TestMethod]
        public void ShouldParseAChainOfLoreAndTales()
        {
            var configuration = new Configuration("configuration");
            var connections = new Configuration("connections");
            var email = new Configuration("email");
            var infrastructure = new Configuration("infrastructure");
            var smtp = new Configuration("smtp", "mass-relay", email);
            var database = new Configuration("database", "localhost", connections);
        }
    }
}
