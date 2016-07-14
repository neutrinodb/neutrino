
using System.IO;

namespace Neutrino.Tests.Stubs {
    public class FileFinderStub : IFileFinder {
        public static string DataSetPath = "DataSets";

        public string GetDataSetPath(string id) {
            return Path.Combine(DataSetPath, id);
        }
    }
}