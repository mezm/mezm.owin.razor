using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;

using Owin;

namespace Mezm.Owin.Razor.Example
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseErrorPage();

            var routes = new RouteTable()
                .AddFileRoute("/", "Views/index.cshtml")
                .AddFileRoute("/about/me", "Views/about.cshtml", new AboutMeModel { Name = "Val" });

            var renderer = new RazorRenderer();
            var middleware = new RazorMiddleware(routes, renderer);

            appBuilder.UseHandlerAsync(middleware.Handle);
        } 
    }
}