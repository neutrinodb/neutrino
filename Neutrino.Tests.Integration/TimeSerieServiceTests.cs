using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Neutrino.Core;
using Neutrino.Data;
using NUnit.Framework;

namespace Neutrino.Tests.Integration {
    public class TimeSerieServiceTests {
        private string _id = "med1";
        private FileFinder _fileFinder;
        private TimeSerieService _service;

        [SetUp]
        public void SetUp() {
            _fileFinder = new FileFinder("DataSets");
            File.Delete(_fileFinder.GetDataSetPath(_id));
            _service = new TimeSerieService(_fileFinder);
        }

        [Test]
        public async void Should_put_ocurrences() {
            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(_fileFinder);
            await service.Create(new TimeSerieHeader(_id, start, end, Interval.OneMinute));

            for (int i = 0; i < 5; i++) {
                await service.Add("med1", new Occurrence { DateTime = start.AddMinutes(i), Value = i });                
            }
            var list = await service.List(_id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);

            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(i, list.Occurrences[i].Value);
            }
        }

        [Test]
        public async void Should_add_ocurrences() {
            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(_fileFinder);
            await service.Create(new TimeSerieHeader(_id, start, end, Interval.OneMinute));

            var occurrences = new List<Occurrence>();
            for (int i = 0; i < 5; i++) {
                occurrences.Add(new Occurrence { DateTime = start.AddMinutes(i), Value = i });
            }
            await service.Add("med1", occurrences);

            var list = await service.List(_id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);

            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(i, list.Occurrences[i].Value);
            }
        }

        [Test]
        public async void Should_list_ocurrences() {
            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(4);
            var service = new TimeSerieService(_fileFinder);
            await service.Create(new TimeSerieHeader(_id, start, end, Interval.OneMinute));

            var list = await service.List(_id, start, end);
            Assert.AreEqual(5, list.Occurrences.Count);
        }

        [Test]
        public async void Should_create_datafile_with_correct_size() {
            await CreateDatafile(5);
            var path = _fileFinder.GetDataSetPath(_id);
            int filesize = DataFile.HeaderSize + DataFile.RegisterSize * 5;
            Assert.AreEqual(filesize, new FileInfo(path).Length);
        }

        [Test]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(1000)]
        public async void Should_extend_datafile(int increaseInMinutes) {
            var header = await CreateDatafile(5);
            await _service.Add(_id, new Occurrence(new DateTime(2010, 1, 1).AddMinutes(increaseInMinutes), 10));
            var path = _fileFinder.GetDataSetPath(_id);
            int filesize = DataFile.HeaderSize + DataFile.RegisterSize * (5 + header.AutoExtendStep);
            Assert.AreEqual(filesize, new FileInfo(path).Length);
        }

        [Test]
        [ExpectedException(typeof(DateAfterLimitException))]
        public async void Should_refuse_to_extend_datafile_when_date_later_than_limit() {
            var header = await CreateDatafile(5);
            await _service.Add(_id, new Occurrence(new DateTime(2010, 1, 1).AddMinutes(header.AutoExtendStep + 5), 10));
        }

        private async Task<TimeSerieHeader> CreateDatafile(int registers) {
            var start = new DateTime(2010, 1, 1);
            var end = start.AddMinutes(registers - 1);
            var service = new TimeSerieService(_fileFinder);
            var header = new TimeSerieHeader(_id, start, end, Interval.OneMinute);
            await service.Create(header);
            return header;
        }

        [Test]
        [Ignore]
        public async void CreateFiles() {
            var start = new DateTime(2010, 1, 1);
            var end = start.AddYears(2);
            int meters = 10000;
            int grandezas = 10;
            for (int i = 0; i < meters; i++) {
                for (int j = 0; j < grandezas; j++) {
                    string tsname = "ts" + i + "_" + j;
                    await _service.Create(new TimeSerieHeader(tsname, start, end, Interval.FiveMinutes));
                    if (i%1000 == 0 && j%1 == 0) {
                        Console.WriteLine("Done: " + tsname);
                    }
                }
            }
        }
    }
}
