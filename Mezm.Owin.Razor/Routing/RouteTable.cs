﻿using System;
using System.Collections.Generic;
using System.Linq;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public class RouteTable : IRouteTable
    {
        private readonly IList<IRoute> routes = new List<IRoute>();

        public IRouteTable AddRoute(IRoute route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }
            
            routes.Add(route);
            return this;
        }

        public IRequestHandler GetHandler(OwinRequest request)
        {
            var route = routes.FirstOrDefault(x => x.CanRoute(request));
            return route != null ? route.GetHandler(request) : null;
        }
    }
}