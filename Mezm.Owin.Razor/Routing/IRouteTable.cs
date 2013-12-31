using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRouteTable
    {
        IRouteTable AddRoute(IRoute route);

        IRequestHandler GetHandler(OwinRequest request);
    }
}