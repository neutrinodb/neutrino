﻿using Neutrino.Data;
using NUnit.Framework;

namespace Neutrino.Tests.Core {
    public class FileFinderTests {

        [Test]
        [TestCase("foo/bar", "DataSets\\foo\\bar")]
        [TestCase("/foo/bar", "DataSets\\foo\\bar")]
        public void Should_return_correct_path(string url, string result) {
            var fileFinder = new FileFinder("DataSets");
            Assert.AreEqual(result, fileFinder.GetDataSetPath(url));
        }
    }
}
