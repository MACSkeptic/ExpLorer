using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ShallowToStringShouldIncludeTheNameAndValueOfTheConfiguration()
        {
            var configuration = new Configuration("connection", "localhost");
            Assert.AreEqual(
                "{MACSkeptic.ExpLorer.Configuration: {connection: localhost}}", configuration.ToString(true));
        }

        [TestMethod]
        public void FullNameShouldBeTheFullyQualifiedNameOfTheConfiguration()
        {
            var connections = new Configuration("connections");
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            var infrastructure = new Configuration("infrastructure", c => c.With(connections));
            Assert.AreEqual("infrastructure", infrastructure.FullName);
            Assert.AreEqual("infrastructure.connections", connections.FullName);
            Assert.AreEqual("infrastructure.connections.database", database.FullName);
        }

        [TestMethod]
        public void ShouldBeAbleToGetAValidConfiguration()
        {
            var connections = new Configuration("connections");
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            Assert.AreSame(database, connections.Get("database"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOrMissingConfigurationException))]
        public void ShouldThrowAnExceptionWhenAConfigurationIsMissing()
        {
            var infrastructure = new Configuration("infrastructure");
            var connections = new Configuration("connections", c => c.BelongingTo(infrastructure));

            try
            {
                connections.Get("database");
            }
            catch (Exception e)
            {
                Assert.AreEqual(
                    "The configuration named [database] was requested, " +
                    "but could not be located, on the base configuration [infrastructure.connections]. " +
                    "Are you missing a key on your configuration file(s)?",
                    e.Message);
                throw;
            }
        }

        [TestMethod]
        public void ShouldBeEqualToAnotherConfigurationIfItHasTheSameFullNameAndValue()
        {
            var connections = new Configuration("connections");
            var database1 = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            var database2 = new Configuration("database", "localhost", c => c.BelongingTo(connections));

            Assert.IsTrue(database2.Equals(database1));
            Assert.IsTrue(database1.Equals(database2));
        }

        [TestMethod]
        public void ShouldNotBeEqualToAnotherConfigurationIfDoesNotHaveTheSameFullNameAndValue()
        {
            var connections = new Configuration("connections");
            var infrastructure = new Configuration("infrastructure");
            var database1 = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            var database2 = new Configuration("database", "localhost", c => c.BelongingTo(infrastructure));
            var database3 = new Configuration("database", "remotehost", c => c.BelongingTo(connections));
            var database4 = new Configuration("databases", "localhost", c => c.BelongingTo(infrastructure));

            Assert.IsFalse(database2.Equals(database1));
            Assert.IsFalse(database1.Equals(database2));
            Assert.IsFalse(database2.Equals(database3));
            Assert.IsFalse(database1.Equals(database3));
            Assert.IsFalse(database2.Equals(database4));
            Assert.IsFalse(database1.Equals(database4));
            Assert.IsFalse(database3.Equals(database1));
            Assert.IsFalse(database3.Equals(database2));
            Assert.IsFalse(database4.Equals(database1));
            Assert.IsFalse(database4.Equals(database2));
        }

        [TestMethod]
        public void NameShouldNotBeCaseSensitive()
        {
            var connections = new Configuration("connections");
            var database = new Configuration("database", "localhost", c => c.BelongingTo(connections));
            Assert.AreEqual(database, connections.Get("DaTabase"));
        }
    }
}