using System;

using Microsoft.Owin.Hosting;

namespace Mezm.Owin.Razor.Example
{
    public class Program
    {
        public static void Main()
        {
            using (WebApp.Start<Startup>(new StartOptions { Port = 5000 }))
            {
                Console.WriteLine("Started at http://localhost:5000");
                Console.ReadLine();
            }
        }
    }
}
