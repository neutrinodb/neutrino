using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data {
    public class TimeSerieService<T> : ITimeSerieService<T> {
        private IFileFinder _fileFinder;
        private IFileStreamOpener _fileStreamOpener;

        public static TimeSerieHeader DefaultTimeSerie;

        public TimeSerieService(IFileFinder fileFinder, IFileStreamOpener fileStreamOpener) {
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
            //var kind = timeSerieHeader.OcurrenceType;
            //using (var ms = new MemoryStream()) {
            //    using (var bw = new BinaryWriter(ms)) {
            //        for (int i = 0; i < timeSerieHeader.TotalLength; i++) {
            //            bw.Write(kind.GetInfo().DefaultValue);
            //        }
            //    }
            //    return ms.ToArray();
            //}
            return null;
        }

        public async Task<TimeSerie<T>> List(string id, DateTime start, DateTime end) {
            //var fullPath = _fileFinder.GetDataSetPath(id);
            //var headerBytes = new byte[TimeSerieHeader.HEADER_SIZE];
            //using (var fs = _fileStreamOpener.OpenWithoutLock(fullPath)) {
            //    await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
            //    var header = TimeSerieHeaderDeserializer.Deserialize(id, headerBytes);
            //    long numberOfRegisters = header.CalcNumberOfRegisters(start, end);
            //    var bodyBytes = new byte[];
            //    return await fs.ReadAsync()
            //}

            //var dataFile = new DataFile(_fileFinder);
            //var headerBytes = await dataFile.ReadHeaderAsync(id);
            //var timeSerieHeader = DeserializeHeader(id, headerBytes);
            return null;
        }

        private static long CalcNumberOfRegisters(DateTime start, DateTime end, long intervalInMillis) {
            return ((long)(end - start).TotalMilliseconds / intervalInMillis) + 1;
        }

        public async Task Add(string id, Occurrence<T> occurrence) {
            await Add(id, new List<Occurrence<T>> { occurrence });
        }

        public async Task Add(string id, List<Occurrence<T>> occurrences) {
            //var dataFile = new DataFile(_fileFinder);
            //byte[] headerBytes;
            //try {
            //    headerBytes = await dataFile.ReadHeaderAsync(id);
            //}
            //catch (Exception fex) when (DefaultTimeSerie != null) {
            //    var header = new TimeSerieHeader(id, DefaultTimeSerie.Start, DefaultTimeSerie.End, DefaultTimeSerie.IntervalInMillis);
            //    await Create(header);
            //    headerBytes = await dataFile.ReadHeaderAsync(id);
            //}
            //var timeSerieHeader = DeserializeHeader(id, headerBytes);

            //var dateEnd = occurrences.Last().DateTime;
            //await EnsureDateRange(timeSerieHeader, dateEnd, dataFile);
            //await dataFile.WriteRegistersAsync(timeSerieHeader, occurrences);
        }

        //private static async Task EnsureDateRange(TimeSerieHeader timeSerieHeader, DateTime dateEnd, DataFile dataFile) {
        //    if (dateEnd <= timeSerieHeader.End) {
        //        return;
        //    }
        //    var extendLimit = timeSerieHeader.IntervalInMillis * timeSerieHeader.AutoExtendStep;
        //    var maxDateAllowed = timeSerieHeader.End.Add(TimeSpan.FromMilliseconds(extendLimit));
        //    if (dateEnd > maxDateAllowed) {
        //        throw new DateAfterLimitException(timeSerieHeader, dateEnd);
        //    }
        //    await dataFile.ExtendFileAsync(timeSerieHeader, dateEnd);
        //}
    }
}