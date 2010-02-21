using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using MACSkeptic.ExpLorer.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests.Proxies
{
    [TestClass]
    public class ConfigurationProxyTest
    {
        [TestMethod]
        public void Proxxy()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", string.Empty, configuration);
            var connections = new Configuration("connections", string.Empty, infrastructure);
            var email = new Configuration("email", string.Empty, infrastructure);
            var smtp = new Configuration("smtp", "mass-relay", email);
            var database = new Configuration("database", "localhost", connections);

            var proxy = ConfigurationProxy.For<IConfiguration>(configuration);
            Assert.AreEqual(database.Value, proxy.Infrastructure.Connections.Database);
            Assert.AreEqual(smtp.Value, proxy.Infrastructure.Email.Smtp);
        }
    }

    public interface IConfiguration
    {
        IInfrastructure Infrastructure { get; set; }
    }

    public interface IInfrastructure
    {
        IConnections Connections { get; set; }
        IEmail Email { get; set; }
    }

    public interface IEmail
    {
        string Smtp { get; set; }
    }

    public interface IConnections
    {
        string Database { get; set; }
    }
}
