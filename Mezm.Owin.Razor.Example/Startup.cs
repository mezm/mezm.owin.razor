using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;

using Microsoft.Owin.FileSystems;

using Owin;

namespace Mezm.Owin.Razor.Example
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseErrorPage();

            var routes = new RouteTable(new PhysicalFileSystem("Views"))
                .AddFileRoute("/", "index.cshtml")
                .AddFileRoute("/about/me", "about.cshtml");

            var renderer = new RazorRenderer();
            var middleware = new RazorMiddleware(routes, renderer);

            appBuilder.UseHandlerAsync(middleware.Handle);
        } 
    }
}