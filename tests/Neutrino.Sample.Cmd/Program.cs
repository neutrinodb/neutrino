using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Neutrino.Client;

namespace Neutrino.Sample.Cmd {
    public class Program {
        public static void Main(string[] args) {
            try {
                var client = new NeutrinoClient(new Uri("http://localhost:34177/", UriKind.Absolute));
                Console.WriteLine("Start");

                var header = new TimeSerieHeader("id1", new DateTime(2016, 01, 01), new DateTime(2018, 01, 01), Interval.FiveMinutes, OccurrenceKind.Decimal, 100);
                client.CreateTimeSerieAsync(header).Wait();
                client.CreateTimeSerieAsync(header).Wait();

                var date = new DateTime(2016, 01, 01);
                var list = new List<OccurrenceDecimal>();
                for (int i = 0; i < 24; i++) {
                    list.Add(new OccurrenceDecimal(date, i));
                    date = date.AddMilliseconds(Interval.FiveMinutes);
                }
                client.SaveAsync("id1", list).Wait();

                var ts = client.ListDecimalAsync("id1", new DateTime(2016, 01, 01), new DateTime(2016, 01, 02)).Result;
                for (int i = 0; i < ts.Occurrences.Count; i++) {
                    Console.WriteLine("-> " + ts.Occurrences[i].DateTime + " " + ts.Occurrences[i].Value);
                }
                Console.WriteLine("End");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }


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