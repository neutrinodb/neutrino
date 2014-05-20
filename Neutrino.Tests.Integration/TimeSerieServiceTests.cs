using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Neutrino.Core;
using Neutrino.Data;
using NUnit.Framework;

namespace Neutrino.Tests.Integration {
    public class TimeSerieServiceTests {

        [Test]
        public async void Test() {
            string id = "med1";
            var fileFinder = new FileFinder("DataSets");
            File.Delete(fileFinder.GetDataSetPath(id));

            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(fileFinder);
            string path = await service.Create(new TimeSerie(id, start, end, Interval.OneMinute));

            var list = await service.List(id, start, end);
            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public async void CreateFiles() {
            var fileFinder = new FileFinder("DataSets");
            var service = new TimeSerieService(fileFinder);
            
            var start = new DateTime(2010, 1, 1);
            var end = start.AddYears(5);
            int meters = 100000;
            int grandezas = 10;
            for (int i = 0; i < meters; i++) {
                for (int j = 0; j < grandezas; j++) {
                    string tsname = "ts" + i + "_" + j;
                    await service.Create(new TimeSerie(tsname, start, end, Interval.FiveMinutes));
                    if (i%1000 == 0 && j%1 == 0) {
                        Console.WriteLine("Done: " + tsname);
                    }
                }
            }
        }
    }
}
