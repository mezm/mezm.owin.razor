using System;
using System.Threading.Tasks;

namespace Mezm.Owin.Razor.Rendering
{
    public class RazorRenderer : IRazorRenderer
    {
        public Task<string> Render(string template, object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentNullException("template");
            }

            return Task.Run(() => RazorEngine.Razor.Parse(template, model));
        }
    }
}