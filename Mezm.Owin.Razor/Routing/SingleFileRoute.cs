using System;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public class SingleFileRoute : IRoute
    {
        private readonly string urlPath;

        private readonly IRequestHandler handler;

        public SingleFileRoute(string urlPath, IRequestHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                throw new ArgumentNullException("urlPath");
            }
            this.urlPath = urlPath;
            this.handler = handler;
        }

        public bool CanRoute(OwinRequest request)
        {
            return string.Equals(request.Path, urlPath, StringComparison.OrdinalIgnoreCase);
        }

        public IRequestHandler GetHandler(OwinRequest request)
        {
            return handler;
        }
    }
}