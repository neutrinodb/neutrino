using System;
using System.Collections.Generic;
using System.IO;
using Neutrino.Core;
using Neutrino.Data;
using NUnit.Framework;

namespace Neutrino.Tests.Integration {
    public class TimeSerieServiceTests {

        [Test]
        public async void Should_put_ocurrences() {
            string id = "med1";
            var fileFinder = new FileFinder("DataSets");
            File.Delete(fileFinder.GetDataSetPath(id));

            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(fileFinder);
            await service.Create(new TimeSerieInfo(id, start, end, Interval.OneMinute));

            for (int i = 0; i < 5; i++) {
                await service.Add("med1", new Occurrence { DateTime = start.AddMinutes(i), Value = i });                
            }
            var list = await service.List(id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);

            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(i, list.Occurrences[i].Value);
            }
        }

        [Test]
        public async void Should_add_ocurrences() {
            string id = "med1";
            var fileFinder = new FileFinder("DataSets");
            File.Delete(fileFinder.GetDataSetPath(id));

            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(fileFinder);
            await service.Create(new TimeSerieInfo(id, start, end, Interval.OneMinute));

            var occurrences = new List<Occurrence>();
            for (int i = 0; i < 5; i++) {
                occurrences.Add(new Occurrence { DateTime = start.AddMinutes(i), Value = i });
            }
            await service.Add("med1", occurrences);                

            var list = await service.List(id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);

            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(i, list.Occurrences[i].Value);
            }
        }

        [Test]
        public async void Should_list_ocurrences() {
            string id = "med1";
            var fileFinder = new FileFinder("DataSets");
            File.Delete(fileFinder.GetDataSetPath(id));

            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(fileFinder);
            await service.Create(new TimeSerieInfo(id, start, end, Interval.OneMinute));

            var list = await service.List(id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);
        }

        [Test]
        [Ignore]
        public async void CreateFiles() {
            var fileFinder = new FileFinder("DataSets");
            var service = new TimeSerieService(fileFinder);
            
            var start = new DateTime(2010, 1, 1);
            var end = start.AddYears(2);
            int meters = 10000;
            int grandezas = 10;
            for (int i = 0; i < meters; i++) {
                for (int j = 0; j < grandezas; j++) {
                    string tsname = "ts" + i + "_" + j;
                    await service.Create(new TimeSerieInfo(tsname, start, end, Interval.FiveMinutes));
                    if (i%1000 == 0 && j%1 == 0) {
                        Console.WriteLine("Done: " + tsname);
                    }
                }
            }
        }
    }
}
