using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using FluentAssertions;

using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;

using Moq;

using NUnit.Framework;

using Owin.Types;

namespace Mezm.Owin.Razor.Tests
{
    [TestFixture]
    public class RazorMiddlewareTests
    {
        private Mock<IRouteTable> table;

        private Mock<IRazorRenderer> renderer;

        private RazorMiddleware middleware;

        private OwinRequest request;

        private OwinResponse response;

        [SetUp]
        public void Init()
        {
            table = new Mock<IRouteTable>();
            renderer = new Mock<IRazorRenderer>();
            request = new OwinRequest(new Dictionary<string, object>());
            response = new OwinResponse(new Dictionary<string, object>()) { Headers = new Dictionary<string, string[]>(), Body = new MemoryStream() };

            middleware = new RazorMiddleware(table.Object, renderer.Object);
        }

        [Test]
        public void HandleRazorRequest()
        {
            var route = new Mock<IRoute>();
            route.Setup(x => x.GetTemplate(request)).Returns(Task.FromResult("the _template_"));
            table.Setup(x => x.GetRoute(request)).Returns(route.Object);
            renderer.Setup(x => x.Render("the _template_", It.IsAny<object>())).Returns(Task.FromResult("*output*"));

            middleware.Handle(request, response, () => Task.Run(() => Assert.Fail())).Wait();

            response.ContentType.Should().Be("text/html");
            using (var reader = new StreamReader(response.Body))
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                reader.ReadToEnd().Should().Be("*output*");
            }
        }

        [Test]
        public void HandleOtherRequest()
        {
            var calledNext = false;
            table.Setup(x => x.GetRoute(request)).Returns((IRoute)null);

            middleware.Handle(request, response, () => Task.Run(() => calledNext = true)).Wait();
            
            calledNext.Should().BeTrue();
        }
    }
}