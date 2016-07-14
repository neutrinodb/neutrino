using Neutrino.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Sample.Cmd {
    public class Program {
        public static void Main(string[] args) {
            //var service = new TimeSerieService(new FileFinder("datasets"));

            //var start = new DateTime(2015, 1, 1);
            //var end = start.AddYears(20);
            //var size = (int)(end - start).TotalMinutes / 5;
            //var series = 100;
            //var items = Enumerable.Range(0, size).Select(x => new Occurrence(start.AddMinutes(x * 5), x)).ToList();

            
            //var cincoMinutos = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;

            //TimeSerieService.DefaultTimeSerie = new TimeSerieHeader("", start, end, cincoMinutos);

            //var sw = new Stopwatch();

            //////creating
            ////for (int i = 0; i < series; i++) {
            ////    var ts = new TimeSerieHeader($"/med/{i}/teste.ts", start, end, cincoMinutos);
            ////    var x = service.Create(ts).Result;
            ////    Console.WriteLine("Creating Loop: " + i);
            ////}

            //var swTotal = new Stopwatch();
            //swTotal.Start();
            ////populating
            //for (int i = 0; i < series; i++) {
            //    sw.Restart();
            //    service.Save($"/med/{i}/teste.ts", items).Wait();
            //    Console.WriteLine($"Save -> {items.Count} registers in {sw.Elapsed.TotalMilliseconds}");
            //}
            //Console.WriteLine($"Total Save -> {items.Count * series} registers in {swTotal.Elapsed.TotalMilliseconds}");
            
            //while (true) {
            //    Console.WriteLine("Query");
            //    Console.WriteLine("Initial date:");
            //    var parIni = DateTime.Parse(Console.ReadLine());
            //    Console.WriteLine("End date:");
            //    var parEnd = DateTime.Parse(Console.ReadLine());

            //    sw.Restart();
            //    var list = service.List($"/med/1/teste.ts", parIni, parEnd).Result;
            //    Console.WriteLine($"Read -> {list.Occurrences.Count} registers in {sw.Elapsed.TotalMilliseconds}");
            //}
        }
    }
}