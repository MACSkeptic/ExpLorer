using System.IO;
using System.Reflection;
using MACSkeptic.ExpLorer.Parsers;
using MACSkeptic.ExpLorer.Proxies;
using MACSkeptic.ExpLorer.Proxies.Interceptors;
using MACSkeptic.ExpLorer.Tests.Proxies.Interfaces;
using MACSkeptic.ExpLorer.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MACSkeptic.ExpLorer.Tests
{
    [TestClass]
    public class SageTest
    {
        [TestMethod]
        public void ShouldRequestTheCorrectFile()
        {
            var parserMock = new Mock<IConfigurationParser>(MockBehavior.Strict);
            var proxyMock = new Mock<IConfigurationProxyProvider>(MockBehavior.Strict);
            var configurationMock = new Mock<IRightAnswerConfiguration>(MockBehavior.Strict);
            var sage = new Sage(parserMock.Object, proxyMock.Object);

            var filePath =
                "#{path}\\#{file}".ApplyArguments(
                    new
                        {
                            path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName,
                            file = "RightAnswerConfiguration"
                        });

            var configuration = new Configuration("RightAnswerConfiguration");
            parserMock
                .Setup(parser => parser.LoadFromFile(filePath))
                .Returns(configuration);

            proxyMock.Setup(proxy => proxy.For(typeof (IRightAnswerConfiguration), configuration)).Returns(configurationMock.Object);
            
            sage.CreateProxy<IRightAnswerConfiguration>();

            parserMock.VerifyAll();
        }

        [TestMethod]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\configuration.lore")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\connections.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\email.tale")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\infrastructure.tale")]
        public void ShouldReadTheCorrectConfigurationFileAsALore()
        {
            var sage = new Sage(new ConfigurationParser(new FileResolver(), "lore", "tale"), new ConfigurationProxyProvider());
            var proxy = sage.CreateProxy<IConfiguration>();
            Assert.AreEqual("42", proxy.Answer); 
        }

        [TestMethod]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\configuration.coffee")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\connections.coffee")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\email.coffee")]
        [DeploymentItem(@"MACSkeptic.ExpLorer.Tests\Fixtures\ConfigurationFiles\FromAssembly\infrastructure.coffee")]
        public void ShouldReadTheCorrectConfigurationFileAsACoffee()
        {
            var sage = new Sage(new ConfigurationParser(new FileResolver(), "coffee"), new ConfigurationProxyProvider());
            var proxy = sage.CreateProxy<IConfiguration>();
            Assert.AreEqual("42", proxy.Answer);
        }
    }
}