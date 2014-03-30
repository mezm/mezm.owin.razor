using Mezm.Owin.Razor.Routing;

using Owin;

namespace Mezm.Owin.Razor.Example
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseErrorPage();
            appBuilder.UseRazor(InitRoutes);
        }

        private static void InitRoutes(IRouteTable table)
        {
            table
                .AddFileRoute("/", "Views/index.cshtml")
                .AddFileRoute("/about/me", "Views/about.cshtml", new AboutMeModel { Name = "Val" })
                .AddFileRoute("/a", "Views/with_layout.cshtml");
        }
    }
}