using System;

using FluentAssertions;

using Mezm.Owin.Razor.Rendering;

using NUnit.Framework;

using RazorEngine.Templating;

namespace Mezm.Owin.Razor.Tests.Rendering
{
    [TestFixture]
    public class RazorRendererTests
    {
        private RazorRenderer renderer;

        [SetUp]
        public void Init()
        {
            renderer = new RazorRenderer();
        }

        [Test]
        public void Render()
        {
            renderer.Render("This is @Model.Text.", new RazorModel { Text = "test" }).Result.Should().Be("This is test.");
        }

        [Test]
        [ExpectedException(typeof(TemplateParsingException))]
        public void RenderFailedCompilationError()
        {
            ExceptionUtil.UnwrapTaskException(() => renderer.Render("This is @.Model.Text.", new RazorModel { Text = "test" }));
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void RenderFailedModelDataIncorrect()
        {
            ExceptionUtil.UnwrapTaskException(() => renderer.Render("This is @Model.Text.Substring(2).", new RazorModel()));
        }

        public class RazorModel
        {
            public string Text { get; set; }
        }
    }
}