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
        private RouteTable table;

        [SetUp]
        public void Init()
        {
            table = new RouteTable(new PhysicalFileSystem(""));
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