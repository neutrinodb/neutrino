using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neutrino.Data;
using NUnit.Framework;

namespace Neutrino.Tests.Core {
    public class FileFinderTests {

        [Test]
        public void Test() {
            var fileFinder = new FileFinder("DataSets");
            Assert.AreEqual("", fileFinder.GetDataSetPath("/foo/bar"));
        }
    }
}
