using System.Threading.Tasks;

using Owin.Types;

namespace Mezm.Owin.Razor.Rendering
{
    public interface IRazorRenderer
    {
        Task Render(string template, object model, OwinResponse response);
    }
}