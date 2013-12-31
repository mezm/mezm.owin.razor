using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRoute
    {
        bool CanRoute(OwinRequest request);

        IRequestHandler GetHandler(OwinRequest request);
    }
}