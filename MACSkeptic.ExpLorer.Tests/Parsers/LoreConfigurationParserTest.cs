using System;
using MACSkeptic.ExpLorer.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Tests.Parsers
{
    [TestClass]
    public class LoreConfigurationParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(TooManyConfigurationFilesException))]
        public void ShouldNotifyAboutAPathWithMultipleLoreFiles()
        {
            var parser = new ConfigurationParser(new FileResolver(), "lore", "tale");
            try
            {
                parser.LoadFromPath(@"Fixtures\ConfigurationFiles\MultipleLores");
            }
            catch(Exception e)
            {
                Assert.AreEqual(@"Found too many configuration files: [Fixtures\ConfigurationFiles\MultipleLores\RightAnswerConfiguration.lore; Fixtures\ConfigurationFiles\MultipleLores\WrongAnswerConfiguration.lore]", e.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NoConfigurationFileException))]
        public void ShouldNotifyAboutAPathWithNoLoreFiles()
        {
            var parser = new ConfigurationParser(new FileResolver(), "lore", "tale");
            try
            {
                parser.LoadFromPath(@"Fixtures\ConfigurationFiles\NoLores");
            }
            catch (Exception e)
            {
                Assert.AreEqual("No configuration file was found on the directory: [#{path}]"
                    .ApplyArguments(new { path = @"Fixtures\ConfigurationFiles\NoLores" }), e.Message);
                throw;
            }
        }

        [TestMethod]
        public void ShouldParseAChainOfLoreAndTales()
        {
            var configuration = new Configuration("configuration");
            var infrastructure = new Configuration("infrastructure", c => c.BelongingTo(configuration));
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));
            var email = new Configuration("email", c => c.BelongingTo(infrastructure));
            var smtp = new Configuration("smtp", "mass-relay", c => c.BelongingTo(email));
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));

            var parser = new ConfigurationParser(new FileResolver(), "lore", "tale");
            var loadedConfiguration = parser.LoadFromPath(@"Fixtures\ConfigurationFiles");

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }


        [TestMethod]
        public void ShouldCorrectlyParseConfigurationValuesThatContainColonOnThem()
        {
            var parser = new ConfigurationParser(new FileResolver(), "lore", "tale");
            var loadedConfiguration = parser.LoadFromPath(@"Fixtures\ConfigurationFiles");

            Assert.AreEqual("42:i:j:k", loadedConfiguration.Get("complexAnswer").Value);
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

            var parser = new ConfigurationParser(new FileResolver(), "lore", "tale");
            var loadedConfiguration = parser.LoadFromCurrentAssembly();

            Assert.AreEqual(smtp, loadedConfiguration.Get("infrastructure").Get("email").Get("smtp"));
            Assert.AreEqual(database, loadedConfiguration.Get("infrastructure").Get("connections").Get("database"));
        }
    }
}