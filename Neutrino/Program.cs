using System;
using Microsoft.Owin.Hosting;
using Neutrino.Api;

namespace Neutrino {
    internal class Program {
        private static void Main(string[] args) {
            using (WebApp.Start<Startup>("http://localhost:12345")) {
                Console.ReadLine();
            }
        }
    }
}