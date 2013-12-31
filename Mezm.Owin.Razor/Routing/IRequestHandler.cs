using System.Threading.Tasks;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public interface IRequestHandler
    {
        Task<string> GetTemplate(OwinRequest request);

        Task<object> GetModel(OwinRequest request);
    }
}