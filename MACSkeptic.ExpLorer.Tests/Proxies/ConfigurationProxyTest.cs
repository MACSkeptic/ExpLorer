using MACSkeptic.ExpLorer.Parsers;
using MACSkeptic.ExpLorer.Proxies;
using MACSkeptic.ExpLorer.Proxies.Interceptors;
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
            var answer = new Configuration("answer", "42");
            var configuration = new Configuration("configuration", c => c.With(answer));
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "mass-relay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            var amqp = new Configuration("amqp", "remotehost", c => c.BelongingTo(connections));
            var proxy = ConfigurationProxy.For<IConfiguration>(configuration);
            Assert.AreEqual(database.Value, proxy.Infrastructure.Connections.Database);
            Assert.AreEqual(amqp.Value, proxy.Infrastructure.Connections.Amqp);
            Assert.AreEqual(smtp.Value, proxy.Infrastructure.Email.Smtp);
            Assert.AreEqual(answer.Value, proxy.Answer);
        }

        [TestMethod]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\configuration.lore")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\connections.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\email.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\infrastructure.tale")]
        public void ProxyForParserShouldRespondCorrectly()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "massive-relay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            var proxy =
                ConfigurationProxy.For<IConfiguration>(new ConfigurationParser(new FileResolver(), "lore", "tale"));
            Assert.AreEqual(database.Value, proxy.Infrastructure.Connections.Database);
            Assert.AreEqual(smtp.Value, proxy.Infrastructure.Email.Smtp);
        }

        [TestMethod]
        public void ProxyShouldProvideASurrogateConfigurationIfNeeded()
        {
            var configuration = new Configuration("configuration");
            var surrogate = new Configuration(
                "surrogate",
                c => c.With(new Configuration("package.onm", "debug"), new Configuration("package.two", "info"))
                         .BelongingTo(configuration));
            var proxy = ConfigurationProxy.For<IConfiguration>(configuration);
            Assert.AreEqual(surrogate, proxy.Surrogate);
        }


        [TestMethod]
        public void ProxyShouldProvideASurrogateConfigurationIfNeededNewMethod()
        {
            var configuration = new Configuration("configuration");
            var surrogate = new Configuration(
                "surrogate",
                c => c.With(new Configuration("package.onm", "debug"), new Configuration("package.two", "info"))
                         .BelongingTo(configuration));
            var proxy = (IConfiguration) new ConfigurationProxyProvider().For(typeof(IConfiguration), configuration);
            Assert.AreEqual(surrogate, proxy.Surrogate);
        }
    }
}