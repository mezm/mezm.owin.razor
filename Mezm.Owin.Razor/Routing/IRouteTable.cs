using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRouteTable
    {
        IRouteTable AddRoute(IRoute route);

        IRouteTable AddFileRoute(string urlPath, string filename);

        IRequestHandler GetHandler(OwinRequest request);
    }
}