using System;
using System.Collections.Generic;
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

        public IRouteTable AddFileRoute(string name, string urlPath, string filename)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                throw new ArgumentNullException("urlPath");
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException("filename");
            }

            var route = new SingleFileRoute(fileSystem, name, urlPath, filename);
            return AddRoute(route);
        }

        public IRoute GetRoute(OwinRequest request)
        {
            return routes.FirstOrDefault(x => x.CanRoute(request));
        }
    }
}