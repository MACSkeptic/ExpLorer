using MACSkeptic.ExpLorer.Parsers;
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
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "mass-relay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));

            var parser = new LoreConfigurationParser();
            var loadedConfiguration = parser.LoadFrom(@"Fixtures\ConfigurationFiles");

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }

        [TestMethod]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\configuration.lore")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\connections.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\email.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\infrastructure.tale")]
        public void ShouldParseAChainOfLoreAndTalesFromTheCurrentAssemblyPath()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "massive-relay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));

            var parser = new LoreConfigurationParser();
            var loadedConfiguration = parser.LoadFromCurrentAssembly();

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }
    }
}