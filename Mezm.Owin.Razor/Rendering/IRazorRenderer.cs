using System.Threading.Tasks;

namespace Mezm.Owin.Razor.Rendering
{
    public interface IRazorRenderer
    {
        Task<string> Render(string template, object model);
    }
}