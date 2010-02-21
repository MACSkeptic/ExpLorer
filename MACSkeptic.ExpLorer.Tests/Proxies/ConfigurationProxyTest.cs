using MACSkeptic.ExpLorer.Parsers;
using MACSkeptic.ExpLorer.Proxies;
using MACSkeptic.ExpLorer.Tests.Proxies.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests.Proxies
{
    [TestClass]
    public class ConfigurationProxyTest
    {
        [TestMethod]
        public void ProxyForConfigurationShouldRespondCorrectly()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", string.Empty, configuration);
            var connections = new Configuration("connections", string.Empty, infrastructure);
            var email = new Configuration("email", string.Empty, infrastructure);
            var smtp = new Configuration("smtp", "mass-relay", email);
            var database = new Configuration("database", "localhost", connections);
            var amqp = new Configuration("amqp", "remotehost", connections);

            var proxy = ConfigurationProxy.For<IConfiguration>(configuration);
            Assert.AreEqual(database.Value, proxy.Infrastructure.Connections.Database);
            Assert.AreEqual(amqp.Value, proxy.Infrastructure.Connections.Amqp);
            Assert.AreEqual(smtp.Value, proxy.Infrastructure.Email.Smtp);
        }

        [TestMethod]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\configuration.lore")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\connections.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\email.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\infrastructure.tale")]
        public void ProxyForParserShouldRespondCorrectly()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", string.Empty, configuration);
            var connections = new Configuration("connections", string.Empty, infrastructure);
            var email = new Configuration("email", string.Empty, infrastructure);
            var smtp = new Configuration("smtp", "massive-relay", email);
            var database = new Configuration("database", "localhost", connections);

            var proxy = ConfigurationProxy.For<IConfiguration>(new LoreConfigurationParser());
            Assert.AreEqual(database.Value, proxy.Infrastructure.Connections.Database);
            Assert.AreEqual(smtp.Value, proxy.Infrastructure.Email.Smtp);
        }
    }
}