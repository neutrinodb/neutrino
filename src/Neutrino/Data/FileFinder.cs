using System;
using System.IO;

namespace Neutrino.Data {
    public class FileFinder : IFileFinder {
        private string _basePath;
        //Maximum number of files in a single folder: 4,294,967,295
        //to defrag file index: contig.exe

        public FileFinder(string basePath) {
            _basePath = basePath;
            Directory.CreateDirectory(_basePath);
        }

        public string GetDataSetPath(string id) {
            id = id.Replace("/", "\\");
            if (Path.IsPathRooted(id)) {
                id = id.Substring(1);
            }
            var fullPath = Path.Combine(_basePath, id);
            return fullPath;
        }
    }
}