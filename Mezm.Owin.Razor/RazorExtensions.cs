using System;

using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;
using Microsoft.Owin.FileSystems;
using Owin;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Mezm.Owin.Razor
{
    public static class RazorExtensions
    {
        public static IAppBuilder UseRazor(this IAppBuilder appBuilder, Action<IRouteTable> initRoutes, IFileSystem fileSystem = null, ITemplateResolver templateResolver = null)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }
            if (initRoutes == null)
            {
                throw new ArgumentNullException("initRoutes");
            }

            var routes = new RouteTable(fileSystem ?? new PhysicalFileSystem(""));
            initRoutes(routes);
            var renderer = new RazorRenderer();
            var config = new TemplateServiceConfiguration
            {
                Resolver = templateResolver ?? new FileSystemTempleteResolver(fileSystem ?? new PhysicalFileSystem(""))
            };
            var templateService = new TemplateService(config);
            RazorEngine.Razor.SetTemplateService(templateService);

            var middleware = new RazorMiddleware(routes, renderer);
            return appBuilder.UseHandlerAsync(middleware.Handle);
        }
    }
}