using System.Collections.Generic;

using FluentAssertions;

using Mezm.Owin.Razor.Routing;

using Moq;

using NUnit.Framework;

using Owin.Types;

namespace Mezm.Owin.Razor.Tests.Routing
{
    [TestFixture]
    public class SingleFileRouteTests
    {
        private SingleFileRoute route;

        private Mock<IRequestHandler> requestHandler;
        
        [SetUp]
        public void Init()
        {
            requestHandler = new Mock<IRequestHandler>();
            route = new SingleFileRoute("/blog/recent", requestHandler.Object);
        }

        [Test]
        public void CanRoute()
        {
            var request = new OwinRequest(new Dictionary<string, object>()) { Path = "/blog/recent" };
            route.CanRoute(request).Should().BeTrue();
        }

        [Test]
        [TestCase("/blog/new")]
        [TestCase("/about/me")]
        [TestCase("/blog/recent.html")]
        [TestCase("/recent")]
        public void CannotRoute(string path)
        {
            var request = new OwinRequest(new Dictionary<string, object>()) { Path = path };
            route.CanRoute(request).Should().BeFalse();
        }

        [Test]
        public void GetHandler()
        {
            route.GetHandler(new OwinRequest()).Should().Be(requestHandler.Object);
        }
    }
}