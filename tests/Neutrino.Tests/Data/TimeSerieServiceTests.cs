using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Neutrino.Data;
using Neutrino.Tests.Stubs;
using NUnit.Framework;
using static Neutrino.Tests.TestUtil;

namespace Neutrino.Tests.Data {
    [TestFixture]
    public class TimeSerieServiceTests {

        private TimeSerieService _service;
        private FileStreamOpenerStub _streamOpener;
        private FileFinder _fileFinder;
        private TimeSerieHeader _header;

        [SetUp]
        public void SetUp() {
            _fileFinder = new FileFinder("DataSet");
            _streamOpener = new FileStreamOpenerStub(_fileFinder);
            _service = new TimeSerieService(_fileFinder, _streamOpener);
            _header = new TimeSerieHeader("t1", Yesterday, Yesterday.AddHours(23), OneHour, OccurrenceKind.Decimal, 24);
        }

        [Test]
        public async Task Should_create_file_with_correct_size() {
            var fileSize = TimeSerieHeader.HEADER_SIZE + _header.OcurrenceType.GetBinarySize() * _header.TotalLength;
            await _service.Create(_header);
            Assert.AreEqual(fileSize, _streamOpener.GetStreamInfo(_header.Id).StreamAfterDispose.Length);
        }

        [Test]
        public async Task Should_save_occurrence() {
            await _service.Create(_header);
            var occ = new Occurrence(Yesterday.AddHours(2), 10);
            await _service.Save(_header.Id, occ);
            var bytes = _streamOpener.GetStreamInfo(_header.Id).StreamAfterDispose;
            var seek = TimeSerieHeader.HEADER_SIZE + _header.OcurrenceType.GetBinarySize() * 2;
            using (var mem = new MemoryStream(bytes)) {
                mem.Seek(seek, SeekOrigin.Begin);
                using (var sr = new BinaryReader(mem)) {
                    Assert.AreEqual(10m, sr.ReadDecimal());
                }
            }
        }

        [Test]
        public async Task Should_auto_extend_file_and_save_occurrence() {
            await _service.Create(_header);
            var occ = new Occurrence(Today.AddHours(1), 10);
            await _service.Save(_header.Id, occ);

            var oldEnd = _header.End;
            var bytes = _streamOpener.GetStreamInfo(_header.Id).StreamAfterDispose;
            var seek = TimeSerieHeader.HEADER_SIZE + _header.OcurrenceType.GetBinarySize() * 25;
            using (var mem = new MemoryStream(bytes)) {
                var header = TimeSerieHeader.Deserialize(_header.Id, bytes);
                Assert.AreEqual(occ.DateTime, header.Current);
                Assert.AreEqual(oldEnd.AddMilliseconds(_header.IntervalInMillis * _header.AutoExtendStep), header.End);

                mem.Seek(seek, SeekOrigin.Begin);
                using (var sr = new BinaryReader(mem)) {
                    Assert.AreEqual(10m, sr.ReadDecimal());
                }
            }
        }

        [Test]
        public async Task Should_autocreate_datafile_when_default_is_configured() {
            TimeSerieService.DefaultTimeSerieHeader = _header;
            var fileSize = TimeSerieHeader.HEADER_SIZE + _header.OcurrenceType.GetBinarySize() * _header.TotalLength;
            var occ = new Occurrence(Yesterday.AddHours(2), 10);
            await _service.Save(_header.Id, occ);
            Assert.AreEqual(fileSize, _streamOpener.GetStreamInfo(_header.Id).StreamAfterDispose.Length);

            var seek = TimeSerieHeader.HEADER_SIZE + _header.OcurrenceType.GetBinarySize() * 2;
            var bytes = _streamOpener.GetStreamInfo(_header.Id).StreamAfterDispose;
            using (var mem = new MemoryStream(bytes)) {
                mem.Seek(seek, SeekOrigin.Begin);
                using (var sr = new BinaryReader(mem)) {
                    Assert.AreEqual(10m, sr.ReadDecimal());
                }
            }
        }

        [Test]
        public async Task Should_list_occurrences() {
            await _service.Create(_header);
            var date = Yesterday;
            var list = new List<Occurrence>();
            for (int i = 0; i < 24; i++) {
                list.Add(new Occurrence(date, (decimal?)i));
                date = date.AddMilliseconds(_header.IntervalInMillis);
            }
            await _service.Save(_header.Id, list);

            var result =
                (await _service.List(_header.Id, Yesterday, Yesterday.AddMilliseconds(_header.IntervalInMillis * (list.Count - 1)))).Occurrences;

            CollectionAssert.AreEqual(list, result);
        }

        [Test]
        public async Task Should_not_list_occurrences_after_pointer() {
            await _service.Create(_header);

            var occ = new Occurrence(Yesterday.AddMilliseconds(_header.IntervalInMillis), 10);
            await _service.Save(_header.Id, occ);

            var result =
                (await _service.List(_header.Id, Yesterday, Today)).Occurrences;

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(Yesterday, result[0].DateTime);
            Assert.IsNull(result[0].Value);
            Assert.AreEqual(occ.DateTime, result[1].DateTime);
            Assert.AreEqual(occ.Value, result[1].Value);
        }

        [Test]
        public async Task Should_return_timeserie_header() {
            await _service.Create(_header);
            var h = await _service.Load(_header.Id);
            Assert.AreEqual(_header.Id, h.Id);
            Assert.AreEqual(_header.AutoExtendStep, h.AutoExtendStep);
            Assert.AreEqual(_header.Current, h.Current);
            Assert.AreEqual(_header.End, h.End);
            Assert.AreEqual(_header.IntervalInMillis, h.IntervalInMillis);
            Assert.AreEqual(_header.OcurrenceType, h.OcurrenceType);
            Assert.AreEqual(_header.Start, h.Start);
        }

        [Test]
        public async Task Should_not_create_timserie_with_invalid_id() {
            var list = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars());
            try {
                foreach (var c in list) {
                    _header.Id = "" + c;
                    await _service.Create(_header);
                    Assert.Fail("Should not permit char " + c);
                }
            }
            catch (InvalidIdException) { }
        }

        [Test]
        public async Task Should_not_create_timserie_with_existing_id() {
            await _service.Create(_header);
            Assert.ThrowsAsync<DuplicateKeyException>(() => _service.Create(_header));
        }
    }
}