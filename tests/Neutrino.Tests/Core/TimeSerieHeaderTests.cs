using System;
using NUnit.Framework;

namespace Neutrino.Tests.Core {
    public class TimeSerieHeaderTests {
        [Test]
        public void Default_extend_step_is_1000_times_the_interval() {
            //var ts = new TimeSerieHeader("id", DateTime.Today, DateTime.Today.AddMinutes(2), Interval.OneMinute);
            //Assert.AreEqual(1000, ts.AutoExtendStep);
        }
    }
}