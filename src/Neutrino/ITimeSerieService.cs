using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Neutrino {
    public interface ITimeSerieHeaderService {
        Task<string> Create(TimeSerieHeader timeSerieHeader);
        Task<TimeSerieHeader> Get(string id);
    }

    public class TimeSerieHeaderService : ITimeSerieHeaderService {
        private IFileFinder _fileFinder;
        private IFileStreamOpener _fileStreamOpener;

        public TimeSerieHeaderService(IFileFinder fileFinder, IFileStreamOpener fileStreamOpener) {
            _fileFinder = fileFinder;
            _fileStreamOpener = fileStreamOpener;
        }

        public async Task<string> Create(TimeSerieHeader timeSerieHeader) {
            var fullPath = _fileFinder.GetDataSetPath(timeSerieHeader.Id);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            var header = timeSerieHeader.Serialize();

            using (var fs = _fileStreamOpener.OpenForCreation(fullPath)) {
                await fs.WriteAsync(header, 0, header.Length);
                var body = SerializeEmptyTimeSerieBody(timeSerieHeader);
                await fs.WriteAsync(body, 0, body.Length);
            }
            return fullPath;
        }

        private byte[] SerializeEmptyTimeSerieBody(TimeSerieHeader timeSerieHeader) {
            throw new NotImplementedException();
        }

        public async Task<TimeSerieHeader> Get(string id) {
            var fullPath = _fileFinder.GetDataSetPath(id);
            var headerBytes = new byte[TimeSerieHeader.HEADER_SIZE];
            using (var fs = _fileStreamOpener.OpenWithoutLock(fullPath)) {
                await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
                return TimeSerieHeader.Deserialize(id, headerBytes);
            }
        }
    }

    public interface ITimeSerieService<T> {
        Task<string> Create(TimeSerieHeader timeSerieHeader);
        Task<TimeSerie<T>> List(string id, DateTime start, DateTime end);
        Task Add(string id, Occurrence<T> occurrence);
        Task Add(string id, List<Occurrence<T>> occurrences);
    }
}