using System.Threading.Tasks;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRoute
    {
        string Name { get; }

        bool CanRoute(OwinRequest request);

        Task<string> GetTemplate(OwinRequest request);
    }
}