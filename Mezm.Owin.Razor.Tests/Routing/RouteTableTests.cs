using System.Collections.Generic;
using System.IO;

using FluentAssertions;

using Mezm.Owin.Razor.Routing;

using Microsoft.Owin.FileSystems;

using Moq;

using NUnit.Framework;

using Owin.Types;

namespace Mezm.Owin.Razor.Tests.Routing
{
    [TestFixture]
    public class RouteTableTests
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
        public void AddRouteAndGetFileHandler()
        {
            var fileInfo = new Mock<IFileInfo>();
            var fileInfoObject = fileInfo.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(true);

            table.AddFileRoute("/a/b", "views\\test.cshtml");
            
            var handler = table.GetHandler(new OwinRequest(new Dictionary<string, object>()) { Path = "/a/b" });
            handler.Should().NotBeNull().And.BeOfType<SimpleRequestHandler>();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void AddFileRouteForNotExistingFile()
        {
            IFileInfo fileInfoObject;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\test.cshtml", out fileInfoObject)).Returns(false);

            table.AddFileRoute("/a/b", "views\\test.cshtml");
        }

        [Test]
        public void AddAndGetCustomRoute()
        {
            var route = new Mock<IRoute>();
            var handler = new Mock<IRequestHandler>();
            route.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);
            route.Setup(x => x.GetHandler(It.IsAny<OwinRequest>())).Returns(handler.Object);

            table.AddRoute(route.Object);

            table.GetHandler(new OwinRequest()).Should().Be(handler.Object);
        }

        [Test]
        public void AddSeveralRoutesGetFirstSuitable()
        {
            var route1 = new Mock<IRoute>();
            var route2 = new Mock<IRoute>();
            var route3 = new Mock<IRoute>();

            var handler = new Mock<IRequestHandler>();

            route1.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(false);
            route2.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);
            route3.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);

            route2.Setup(x => x.GetHandler(It.IsAny<OwinRequest>())).Returns(handler.Object);

            table.AddRoute(route1.Object).AddRoute(route2.Object).AddRoute(route3.Object);

            table.GetHandler(new OwinRequest()).Should().Be(handler.Object);
        }

        [Test]
        public void GetRouteEmptyTable()
        {
            table.GetHandler(new OwinRequest()).Should().BeNull();
        }

        [Test]
        public void GetNoSuitableRoutes()
        {
            var route1 = new Mock<IRoute>();
            var route2 = new Mock<IRoute>();
            var route3 = new Mock<IRoute>();

            route1.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(false);
            route2.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(false);
            route3.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(false);

            table.AddRoute(route1.Object).AddRoute(route2.Object).AddRoute(route3.Object);

            table.GetHandler(new OwinRequest()).Should().BeNull();
        }
    }
}