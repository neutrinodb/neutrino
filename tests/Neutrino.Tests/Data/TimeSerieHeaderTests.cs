using System;
using Neutrino.Data;
using Neutrino.Tests.Stubs;
using NUnit.Framework;
using static Neutrino.Tests.TestUtil;

namespace Neutrino.Tests.Data {
    public class TimeSerieHeaderTests {
        private TimeSerieHeader _header;

        [SetUp]
        public void SetUp() {
            _header = new TimeSerieHeader("t1", Yesterday, DateTime.Today, OneHour, OccurrenceKind.Decimal, 24);
        }

        [Test]
        public void Should_serialize_header() {
            var bytes = _header.Serialize();
            var header = TimeSerieHeader.Deserialize(_header.Id, bytes);
            Assert.AreEqual(_header.Id, header.Id);
            Assert.AreEqual(_header.Current, header.Current);
            Assert.AreEqual(_header.Start, header.Start);
            Assert.AreEqual(_header.End, header.End);
            Assert.AreEqual(_header.OcurrenceType, header.OcurrenceType);
            Assert.AreEqual(_header.TotalLength, header.TotalLength);
            Assert.AreEqual(_header.AutoExtendStep, header.AutoExtendStep);
            Assert.AreEqual(_header.IntervalInMillis, header.IntervalInMillis);
        }
    }
}