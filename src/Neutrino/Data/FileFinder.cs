using System;
using System.IO;

namespace Neutrino.Data {
    public class FileFinder : IFileFinder {
        public string BasePath { get; }
        //Maximum number of files in a single folder: 4,294,967,295
        //to defrag file index: contig.exe

        public static string FileSufix = ".ts1";

        public FileFinder(string basePath) {
            if (String.IsNullOrEmpty(basePath)) {
                basePath = ".";
            }
            BasePath = basePath;
            Directory.CreateDirectory(BasePath);
        }

        public string GetDataSetPath(string id) {
            id = id.Replace("/", "\\") + FileSufix;
            try {
                if (Path.IsPathRooted(id)) {
                    id = id.Substring(1);
                }
            }
            catch (ArgumentException ex) {
                throw new InvalidIdException(ex, id);
            }
            var fullPath = Path.Combine(BasePath, id);
            return fullPath;
        }
    }
}