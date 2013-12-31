using System;
using System.Text;
using System.Threading.Tasks;

using Mezm.Owin.Razor.Rendering;
using Mezm.Owin.Razor.Routing;

using Owin.Types;

namespace Mezm.Owin.Razor
{
    public class RazorMiddleware
    {
        private readonly IRouteTable routeTable;

        private readonly IRazorRenderer razorRenderer;

        public RazorMiddleware(IRouteTable routeTable, IRazorRenderer razorRenderer)
        {
            if (routeTable == null)
            {
                throw new ArgumentNullException("routeTable");
            }
            if (razorRenderer == null)
            {
                throw new ArgumentNullException("razorRenderer");
            }
            this.routeTable = routeTable;
            this.razorRenderer = razorRenderer;
        }

        public async Task Handle(OwinRequest request, OwinResponse response, Func<Task> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            var handler = routeTable.GetHandler(request);
            if (handler == null)
            {
                await next();
                return;
            }

            var template = await handler.GetTemplate(request);
            var model = await handler.GetModel(request);
            var output = await razorRenderer.Render(template, model);

            response.ContentType = "text/html";
            var buffer = Encoding.Default.GetBytes(output);
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}