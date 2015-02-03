using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neutrino.Core;
using NUnit.Framework;

namespace Neutrino.Tests.Core {
    public class TimeSerieInfoTests {

        [Test]
        public void Default_extend_step_is_1000_times_the_interval() {
            var ts = new TimeSerieInfo("id", DateTime.Today, DateTime.Today.AddMinutes(2), Interval.OneMinute);
            Assert.AreEqual(1000, ts.AutoExtendStep);
        }   
    }
}
