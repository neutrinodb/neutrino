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
            string subdir = GetSubdir(id);
            return _basePath + Path.DirectorySeparatorChar + subdir + Path.DirectorySeparatorChar + String.Join("", id.Split(Path.GetInvalidFileNameChars())) + ".ts";
        }

        private string GetSubdir(string id) {
            int num = 0;
            for (int i = 0; i < id.Length; i++) {
                num+= id[i];
            }
            string subdir = "hash" + num % 1000;
            if (!Directory.Exists(_basePath + Path.DirectorySeparatorChar + subdir)) {
                Directory.CreateDirectory(_basePath + Path.DirectorySeparatorChar + subdir);
            }
            return subdir;
        }
    }
}