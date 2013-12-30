using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRouteTable
    {
        IRouteTable AddRoute(IRoute route);

        IRouteTable AddFileRoute(string name, string urlPath, string filename);

        IRoute GetRoute(OwinRequest request);
    }
}