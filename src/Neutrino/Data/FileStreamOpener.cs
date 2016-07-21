using System.IO;

namespace Neutrino.Data {
    public class FileStreamOpener : IFileStreamOpener {
        public Stream OpenWithoutLock(string path) {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true);
        }

        public Stream OpenWithLock(string path) {
            return new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 4096, true);
        }

        public Stream OpenForCreation(string path) {
            return new FileStream(path, FileMode.CreateNew);
        }

        public bool FileExists(string path) {
            return File.Exists(path);
        }
    }
}