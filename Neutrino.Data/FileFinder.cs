using System;
using System.IO;
using Neutrino.Core;

namespace Neutrino.Data {
    public class FileFinder : IFileFinder {
        private string _basePath;
        //Maximum number of files in a single folder: 4,294,967,295
        //to defrag file index: contig.exe

        public FileFinder(string basePath) {
            _basePath = basePath;
            if (!Directory.Exists(_basePath)) {
                Directory.CreateDirectory(_basePath);
            }
        }

        public string GetDataSetPath(string id) {
            return String.Format("{0}{1}{2}{1}{3}.ts", 
                _basePath, 
                Path.DirectorySeparatorChar, 
                id, 
                String.Join("", id.Split(Path.GetInvalidFileNameChars())));
        }
    }
}