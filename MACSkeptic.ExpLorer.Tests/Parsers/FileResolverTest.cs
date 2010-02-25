using MACSkeptic.ExpLorer.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MACSkeptic.ExpLorer.Tests.Parsers
{
    [TestClass]
    public class FileResolverTest
    {
        private FileResolver _resolver;

        [TestInitialize]
        public void Setup()
        {
            _resolver = new FileResolver();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPathException))]
        public void ShouldThrowExceptionWhenPathIsEmpty()
        {
            _resolver.Resolve(string.Empty, string.Empty);
        }

        [TestMethod]
        public void ShouldResolveAFileGivenItsName()
        {
            var fileInfo = _resolver.Resolve("FileResolver\\email", "coffee");
            
            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        public void ShouldResolveAFileSupressingTheConfigurationWord()
        {
            var fileInfo = _resolver.Resolve("FileResolver\\emailConfiguration", "coffee");

            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        public void ShouldReturnAFileFromItsParentDirectoryIfDoesNotExistsOnCurrentDirectory()
        {
            var fileInfo = _resolver.Resolve("FileResolver\\connections", "coffee");

            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(NoConfigurationFileException))]
        public void ShouldThrowExceptionWhenFileDoesNotExist()
        {
            _resolver.Resolve("not-exists", "ext");
        }
    }
}
