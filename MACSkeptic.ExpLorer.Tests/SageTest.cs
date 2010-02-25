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
        private MockFactory _factory;
        private Sage _sage;
        private Mock<IConfigurationParser> _parserMock;
        private Mock<IConfigurationProxyProvider> _proxyMock;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MockFactory(MockBehavior.Strict);
            _parserMock = _factory.Create<IConfigurationParser>();
            _proxyMock = _factory.Create<IConfigurationProxyProvider>();
            
            _sage = new Sage(_parserMock.Object, _proxyMock.Object);
        }

        [TestMethod]
        public void TearDown()
        {
            _factory.VerifyAll();
        }

        [TestMethod]
        public void ShouldRequestTheCorrectFile()
        {
            var configurationMock = _factory.Create<IRightAnswerConfiguration>();

            var filePath =
                "#{path}\\#{file}".ApplyArguments(
                    new
                        {
                            path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName,
                            file = "RightAnswerConfiguration"
                        });

            var configuration = new Configuration("RightAnswerConfiguration");
            _parserMock
                .Setup(parser => parser.LoadFromFile(filePath))
                .Returns(configuration);

            _proxyMock.Setup(proxy => proxy.For(typeof (IRightAnswerConfiguration), configuration)).Returns(configurationMock.Object);
            
            _sage.CreateProxy<IRightAnswerConfiguration>();

            _parserMock.VerifyAll();
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

        [TestMethod]
        public void ShouldReturnDefaultValueWhenFileDoesNotExists()
        {
            _parserMock.Setup(
                parser => parser.LoadFromFile(It.IsAny<string>())).Throws(new NoConfigurationFileException(""));

            var configurationMock = _factory.Create<IConfiguration>();
            var configuration = _sage.CreateProxy(configurationMock.Object);

            Assert.AreSame(configurationMock.Object, configuration);
        }
    }
}