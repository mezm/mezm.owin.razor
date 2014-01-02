using System;

using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;

using Owin;

namespace Mezm.Owin.Razor
{
    public static class RazorExtensions
    {
        public static IAppBuilder UseRazor(this IAppBuilder appBuilder, Action<IRouteTable> initRoutes)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }
            if (initRoutes == null)
            {
                throw new ArgumentNullException("initRoutes");
            }

            var routes = new RouteTable();
            initRoutes(routes);
            var renderer = new RazorRenderer();
            var middleware = new RazorMiddleware(routes, renderer);
            return appBuilder.UseHandlerAsync(middleware.Handle);
        }
    }
}