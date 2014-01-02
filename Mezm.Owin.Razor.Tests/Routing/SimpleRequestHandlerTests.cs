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
    public class SimpleRequestHandlerTests
    {
        [Test]
        public void GetTemplate()
        {
            var file = new Mock<IFileInfo>();
            var handler = new SimpleRequestHandler(file.Object, x => new object());

            using (var stream = new MemoryStream(Encoding.Default.GetBytes("The test template @Model.Name.")))
            {
                file.Setup(x => x.CreateReadStream()).Returns(stream);
                handler.GetTemplate(new OwinRequest()).Result.Should().Be("The test template @Model.Name.");
            }
        }

        [Test]
        public void GetModel()
        {
            var handler = new SimpleRequestHandler(new Mock<IFileInfo>().Object, x => "abc-def");
            handler.GetModel(new OwinRequest()).Result.Should().Be("abc-def");
        }

        [Test]
        public void GetIdentity()
        {
            var file = new Mock<IFileInfo>();
            file.Setup(x => x.PhysicalPath).Returns("c:\\tmp\\template.cshtml");
            var handler = new SimpleRequestHandler(file.Object, x => "abc-def");
            handler.GetIdentity(new OwinRequest()).Should().Be("c:\\tmp\\template.cshtml");
        }
    }
}