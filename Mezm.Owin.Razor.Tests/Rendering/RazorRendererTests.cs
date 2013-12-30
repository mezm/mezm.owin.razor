using System;
using System.Linq;

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
            UnwrapTaskException(() => renderer.Render("This is @.Model.Text.", new RazorModel { Text = "test" }).Wait());
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void RenderFailedModelDataIncorrect()
        {
            UnwrapTaskException(() => renderer.Render("This is @Model.Text.Substring(2).", new RazorModel()).Wait());
            Assert.Fail();
        }

        private static void UnwrapTaskException(Action action)
        {
            try
            {
                action();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerExceptions.First();
            }
        }

        public class RazorModel
        {
            public string Text { get; set; }
        }
    }
}