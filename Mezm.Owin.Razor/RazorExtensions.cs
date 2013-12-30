using Owin;

namespace Mezm.Owin.Razor
{
    public static class RazorExtensions
    {
        public static IAppBuilder UseRazor(this IAppBuilder appBuilder)
        {
            return appBuilder;
        }
    }
}