using System.Collections.Generic;
using System.IO;
using System.Text;

using FluentAssertions;

using Mezm.Owin.Razor.Routing;

using Microsoft.Owin.FileSystems;

using Moq;

using NUnit.Framework;

using Owin.Types;

namespace Mezm.Owin.Razor.Tests.Routing
{
    [TestFixture]
    public class SingleFileRouteTests
    {
        private SingleFileRoute route;

        private Mock<IFileSystem> fileSystem;
        
        [SetUp]
        public void Init()
        {
            fileSystem = new Mock<IFileSystem>();
            route = new SingleFileRoute(fileSystem.Object, "test", "/blog/recent", "views\\recent.cshtml");
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
        public void GetRequestSuccess()
        {
            var fileInfoMock = new Mock<IFileInfo>();
            var fileInfo = fileInfoMock.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\recent.cshtml", out fileInfo)).Returns(true);

            using (var stream = new MemoryStream(Encoding.Default.GetBytes("The test template @Model.Name.")))
            {
                fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
                route.GetTemplate(new OwinRequest()).Result.Should().Be("The test template @Model.Name.");
            }
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void GetRequestFailed()
        {
            var fileInfoMock = new Mock<IFileInfo>();
            var fileInfo = fileInfoMock.Object;
            fileSystem.Setup(x => x.TryGetFileInfo("views\\recent.cshtml", out fileInfo)).Returns(false);

            ExceptionUtil.UnwrapTaskException(() => route.GetTemplate(new OwinRequest()));
        }
    }
}