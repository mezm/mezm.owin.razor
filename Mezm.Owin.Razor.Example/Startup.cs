using Mezm.Owin.Razor.Routing;

using Owin;

namespace Mezm.Owin.Razor.Example
{
    using Microsoft.Owin.FileSystems;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseErrorPage();
            appBuilder.UseRazor(InitFileViewRoutes);
            appBuilder.UseRazor(InitEmbeddedViewRoutes, new EmbeddedResourceFileSystem());
        }

        private static void InitFileViewRoutes(IRouteTable table)
        {
            table
                .AddFileRoute("/", "Views/index.cshtml")
                .AddFileRoute("/about/me", "Views/about.cshtml", new AboutMeModel { Name = "Val" })
                .AddFileRoute("/a", "Views/with_layout.cshtml");
        }

        private static void InitEmbeddedViewRoutes(IRouteTable table)
        {
            table.AddFileRoute("/embedded", "/Mezm.Owin.Razor.Example.EmbeddedViews.index.cshtml");
        }
    }
}