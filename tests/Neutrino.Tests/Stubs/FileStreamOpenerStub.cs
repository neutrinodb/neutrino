using System;
using System.Collections.Generic;
using System.IO;
using Neutrino.Data;

namespace Neutrino.Tests.Stubs {
    public class FileStreamOpenerStub : IFileStreamOpener {
        private readonly FileFinder _fileFinder;
        private Dictionary<string, StubStream> _streamsInfo { get; } = new Dictionary<string, StubStream>();

        public FileStreamOpenerStub(FileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public Stream OpenWithoutLock(string path) {
            if (!_streamsInfo.ContainsKey(path)) {
                throw new FileNotFoundException(path);
            }
            var bytes = _streamsInfo[path].StreamAfterDispose;
            _streamsInfo[path] = new StubStream();
            _streamsInfo[path].Write(bytes, 0, bytes.Length);
            _streamsInfo[path].Seek(0, SeekOrigin.Begin);
            return _streamsInfo[path];
        }

        public Stream OpenWithLock(string path) {
            if (!_streamsInfo.ContainsKey(path)) {
                throw new FileNotFoundException(path);
            }
            var bytes = _streamsInfo[path].StreamAfterDispose;
            _streamsInfo[path] = new StubStream();
            _streamsInfo[path].Write(bytes, 0, bytes.Length);
            _streamsInfo[path].Seek(0, SeekOrigin.Begin);
            return _streamsInfo[path];
        }

        public Stream OpenForCreation(string path) {
            var stream = new StubStream();
            _streamsInfo[path] = stream;
            return stream;
        }

        public bool FileExists(string path) {
            return _streamsInfo.ContainsKey(path);
        }

        public StubStream GetStreamInfo(string timeSerieId) {
            var key = _fileFinder.GetDataSetPath(timeSerieId);
            return _streamsInfo[key];
        }
    }

    public class StubStream : MemoryStream {
        public byte[] StreamAfterDispose { get; set; }
        public StubStream() {}
        public StubStream(byte[] bytes) : base(bytes) { }

        protected override void Dispose(bool disposing) {
            StreamAfterDispose = ToArray();
            base.Dispose(disposing);
        }
    }
}