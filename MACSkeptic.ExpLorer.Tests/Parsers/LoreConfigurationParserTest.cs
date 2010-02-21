﻿using MACSkeptic.ExpLorer.Parsers;
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
            var infrastructure = new Configuration("infrastructure", string.Empty, configuration);
            var connections = new Configuration("connections", string.Empty, infrastructure);
            var email = new Configuration("email", string.Empty, infrastructure);
            var smtp = new Configuration("smtp", "mass-relay", email);
            var database = new Configuration("database", "localhost", connections);

            var parser = new LoreConfigurationParser();
            var loadedConfiguration = parser.LoadFrom(@"Fixtures\ConfigurationFiles");

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }
    }
}