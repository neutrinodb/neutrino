using System;
using System.IO;
using Neutrino.Core;

namespace Neutrino.Data {
    public class FileFinder : IFileFinder {
        //Maximum number of files in a single folder: 4,294,967,295
        //to defrag file index: contig.exe
        public string GetDataSetBasePath(string id) {
            string place = "DataSets";
            if (!Directory.Exists(place)) {
                Directory.CreateDirectory(place);
            }
            return place;
        }
    }
}