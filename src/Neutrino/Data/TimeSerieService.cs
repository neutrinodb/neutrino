using Neutrino;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data {
    public class TimeSerieService : ITimeSerieService {
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
            using (var ms = new MemoryStream()) {
                using (var bw = new BinaryWriter(ms)) {
                    for (int i = 0; i < timeSerieHeader.TotalLength; i++) {
                        timeSerieHeader.OcurrenceType.WriteDefaultToStream(bw);
                    }
                }
                return ms.ToArray();
            }
        }

        public async Task<TimeSerie> List(string id, DateTime start, DateTime end) {
            var fullPath = _fileFinder.GetDataSetPath(id);
            var headerBytes = new byte[TimeSerieHeader.HEADER_SIZE];
            var bodyBytes = new byte[0];
            TimeSerieHeader header;
            long numberOfRegisters;
            using (var fs = _fileStreamOpener.OpenWithoutLock(fullPath)) {
                await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
                header = TimeSerieHeader.Deserialize(id, headerBytes);
                numberOfRegisters = header.CalcNumberOfRegisters(start, end);
                var registerSize = header.OcurrenceType.GetBinarySize();
                bodyBytes = new byte[numberOfRegisters * registerSize];
                var seek = header.GetIndex(start);
                fs.Seek(seek * registerSize, SeekOrigin.Current);
                await fs.ReadAsync(bodyBytes, 0, bodyBytes.Length);
            }
            return DeserializeRegisters(bodyBytes, start, numberOfRegisters, header);
        }

        private static TimeSerie DeserializeRegisters(byte[] data, DateTime start, long numberOfRegisters, TimeSerieHeader header) {
            var list = new List<Occurrence>();
            var type = header.OcurrenceType;
            using (var br = new BinaryReader(new MemoryStream(data))) {
                var current = start;
                for (int i = 0; i < numberOfRegisters; i++) {
                    current = current.AddMilliseconds(header.IntervalInMillis);
                    list.Add(new Occurrence(current, type.ReadFromStream(br)));
                }
            }
            return new TimeSerie(header.IntervalInMillis, list);
        }
      
        public async Task Add(string id, Occurrence occurrence) {
            await Add(id, new List<Occurrence> { occurrence });
        }

        public async Task Add(string id, List<Occurrence> occurrences) {
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