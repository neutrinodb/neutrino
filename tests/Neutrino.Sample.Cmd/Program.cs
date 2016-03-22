using Neutrino.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Sample.Cmd {
    public class Program {
        public static void Main(string[] args) {
            var service = new TimeSerieService(new FileFinder("datasets"));

            var items = Enumerable.Range(0, 288).Select(x => new Occurrence(DateTime.Today.AddMinutes(x * 5), x)).ToList();

            var start = new DateTime(2015, 1, 1);
            var end = start.AddYears(3);
            var cincoMinutos = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            TimeSerieService.DefaultTimeSerie = new TimeSerieHeader("", start, end, cincoMinutos);


            //Task.Run(async () => {
                for (int i = 0; i < 100; i++) {
                    service.Add($"/med/{1}/teste.ts", items);
                    Console.WriteLine("Loop: " + i);
                }
            //});

            Console.WriteLine("wait");
            Console.Read();
        }

    }
}