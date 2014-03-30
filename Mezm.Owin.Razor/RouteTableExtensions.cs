using System;
using System.Globalization;
using System.IO;

using Mezm.Owin.Razor.Routing;

using Microsoft.Owin.FileSystems;

using Owin.Types;

namespace Mezm.Owin.Razor
{
    public static class RouteTableExtensions
    {
        public static IRouteTable AddFileRoute(this IRouteTable routeTable, string urlPath, string filename, object model = null)
        {
            return AddFileRoute(routeTable, urlPath, filename, x => model ?? new object());
        }

        public static IRouteTable AddFileRoute(this IRouteTable routeTable, string urlPath, string filename, Func<OwinRequest, object> modelProvider)
        {
            if (routeTable == null)
            {
                throw new ArgumentNullException("routeTable");
            }
            if (modelProvider == null)
            {
                throw new ArgumentNullException("modelProvider");
            }
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                throw new ArgumentNullException("urlPath");
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException("filename");
            }

            IFileInfo fileInfo;
            if (!routeTable.FileSystem.TryGetFileInfo(filename, out fileInfo))
            {
                throw new IOException(string.Format(CultureInfo.CurrentCulture, "File '{0}' was not found.", filename));
            }

            var handler = new SimpleRequestHandler(fileInfo, modelProvider);
            var route = new SingleFileRoute(urlPath, handler);

            return routeTable.AddRoute(route);
        }
    }
}