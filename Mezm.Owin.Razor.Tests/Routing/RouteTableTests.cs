using System.Collections.Generic;

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
        public void AddAndGetFileRoute()
        {
            table.AddFileRoute("test", "/a/b", "views\\test.cshtml");
            
            var route = table.GetRoute(new OwinRequest(new Dictionary<string, object>()) { Path = "/a/b" });
            route.Should().NotBeNull();
            route.Name.Should().Be("test");
        }

        [Test]
        public void AddAndGetCustomRoute()
        {
            var route = new Mock<IRoute>();
            route.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);

            table.AddRoute(route.Object);

            table.GetRoute(new OwinRequest()).Should().Be(route.Object);
        }

        [Test]
        public void AddSeveralRoutesGetFirstSuitable()
        {
            var route1 = new Mock<IRoute>();
            var route2 = new Mock<IRoute>();
            var route3 = new Mock<IRoute>();

            route1.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(false);
            route2.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);
            route3.Setup(x => x.CanRoute(It.IsAny<OwinRequest>())).Returns(true);

            table.AddRoute(route1.Object).AddRoute(route2.Object).AddRoute(route3.Object);

            table.GetRoute(new OwinRequest()).Should().Be(route2.Object);
        }

        [Test]
        public void GetRouteEmptyTable()
        {
            table.GetRoute(new OwinRequest()).Should().BeNull();
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

            table.GetRoute(new OwinRequest()).Should().BeNull();
        }
    }
}