using MACSkeptic.ExpLorer.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests.Parsers
{
    [TestClass]
    public class CoffeeConfigurationParserTest
    {
        [TestMethod]
        public void ShouldParseAChainOfCoffeeWithSugar()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "massive-ultrarelay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));

            var parser = new ConfigurationParser(new FileResolver(), "coffee");
            var loadedConfiguration = parser.LoadFromFile(@"Fixtures\ConfigurationFiles\configuration");

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }
    }
}