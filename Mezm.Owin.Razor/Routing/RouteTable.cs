using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.Owin.FileSystems;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public class RouteTable : IRouteTable
    {
        private readonly IFileSystem fileSystem;

        private readonly IList<IRoute> routes = new List<IRoute>();

        public RouteTable(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            this.fileSystem = fileSystem;
        }

        public IRouteTable AddRoute(IRoute route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }
            
            routes.Add(route);
            return this;
        }

        public IRouteTable AddFileRoute(string urlPath, string filename)
        {
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                throw new ArgumentNullException("urlPath");
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException("filename");
            }

            IFileInfo fileInfo;
            if (!fileSystem.TryGetFileInfo(filename, out fileInfo))
            {
                throw new IOException(string.Format(CultureInfo.CurrentCulture, "File '{0}' was not found.", filename));
            }

            var handler = new SimpleRequestHandler(fileInfo, x => new object());
            var route = new SingleFileRoute(urlPath, handler);
            return AddRoute(route);
        }

        public IRequestHandler GetHandler(OwinRequest request)
        {
            var route = routes.FirstOrDefault(x => x.CanRoute(request));
            return route != null ? route.GetHandler(request) : null;
        }
    }
}