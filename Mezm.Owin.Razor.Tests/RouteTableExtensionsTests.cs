using System.Collections.Generic;
using System.IO;

using FluentAssertions;

using Mezm.Owin.Razor.Routing;

using Microsoft.Owin.FileSystems;

using Moq;

using NUnit.Framework;

using Owin.Types;

namespace Mezm.Owin.Razor.Tests
{
    [TestFixture]
    public class RouteTableExtensionsTests
    {
        private Mock<IFileSystem> fileSystem;

        private RouteTable table;

        [SetUp]
        public void Init()
        {
            fileSystem = new Mock<IFileSystem>();
            table = new RouteTable(fileSystem.Object);
        }

        [Test]
        public void AddFileRoute()
        {
            var fileInfo = new Mock<IFileInfo>();
            var fileInfoObject = fileInfo.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(true);

            table.AddFileRoute("/a/b", "views\\test.cshtml");

            var handler = table.GetHandler(new OwinRequest(new Dictionary<string, object>()) { Path = "/a/b" });
            handler.Should().NotBeNull().And.BeOfType<SimpleRequestHandler>();
            handler.GetModel(new OwinRequest()).Result.Should().BeOfType<object>();
        }

        [Test]
        public void AddFileRouteWithModel()
        {
            var fileInfo = new Mock<IFileInfo>();
            var fileInfoObject = fileInfo.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(true);

            table.AddFileRoute("/a/b", "views\\test.cshtml", 45.3);

            var handler = table.GetHandler(new OwinRequest(new Dictionary<string, object>()) { Path = "/a/b" });
            handler.Should().NotBeNull().And.BeOfType<SimpleRequestHandler>();
            handler.GetModel(new OwinRequest()).Result.Should().Be(45.3);
        }

        [Test]
        public void AddFileRouteWithModelProvider()
        {
            var fileInfo = new Mock<IFileInfo>();
            var fileInfoObject = fileInfo.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(true);

            table.AddFileRoute("/a/b", "views\\test.cshtml", x => x.LocalPort);

            var handler = table.GetHandler(new OwinRequest(new Dictionary<string, object>()) { Path = "/a/b" });
            handler.Should().NotBeNull().And.BeOfType<SimpleRequestHandler>();
            handler.GetModel(new OwinRequest(new Dictionary<string, object>()) { LocalPort = "123" }).Result.Should().Be("123");
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void AddFileRouteForNotExistingFile()
        {
            IFileInfo fileInfoObject;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(false);

            table.AddFileRoute("/a/b", "views\\test.cshtml");
        }
    }
}