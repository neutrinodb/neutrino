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
            var fileFinder = new Mock<IFileFinder>();
            fileFinder.Setup(x => x.GetDataSetBasePath(It.IsAny<string>())).Returns("DataSets");

            var service = new TimeSerieService(fileFinder.Object);
            string path = await service.Create(new TimeSerie("med1", new DateTime(2010, 1, 1), new DateTime(2015, 1, 1),Interval.FiveMinutes));
        }
    }
}
