using System;

using Microsoft.Owin.Hosting;

namespace Mezm.Owin.Razor.Example
{
    public class Program
    {
        public static void Main()
        {
            using (WebApp.Start<Startup>(new StartOptions("http://localhost:9999")))
            {
                Console.WriteLine("Started at http://localhost:9999");
                Console.ReadLine();
            }
        }
    }
}
