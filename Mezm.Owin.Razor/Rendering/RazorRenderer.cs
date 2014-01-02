using System;
using System.Threading.Tasks;

namespace Mezm.Owin.Razor.Rendering
{
    public class RazorRenderer : IRazorRenderer
    {
        public Task<string> Render(string template, object model, string templateIdentity = null)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentNullException("template");
            }

            return
                Task.Run(
                    () =>
                        string.IsNullOrWhiteSpace(templateIdentity)
                            ? RazorEngine.Razor.Parse(template, model)
                            : RazorEngine.Razor.Parse(template, model, templateIdentity));
        }
    }
}