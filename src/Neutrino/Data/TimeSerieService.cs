using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data {
    public class TimeSerieService : ITimeSerieService {
        private IFileFinder _fileFinder;
        private IFileStreamOpener _fileStreamOpener;

        public static TimeSerieHeader DefaultTimeSerieHeader;

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

        public async Task<TimeSerieHeader> Load(string id) {
            var fullPath = _fileFinder.GetDataSetPath(id);
            var headerBytes = new byte[TimeSerieHeader.HEADER_SIZE];
            using (var fs = _fileStreamOpener.OpenWithoutLock(fullPath)) {
                await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
                return TimeSerieHeader.Deserialize(id, headerBytes);
            }
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
            byte[] bodyBytes;
            TimeSerieHeader header;
            long numberOfRegisters;
            using (var fs = _fileStreamOpener.OpenWithoutLock(fullPath)) {
                await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
                header = TimeSerieHeader.Deserialize(id, headerBytes);
                if (end > header.Current) {
                    end = header.Current;
                }
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
                    list.Add(new Occurrence(current, type.ReadFromStream(br)));
                    current = current.AddMilliseconds(header.IntervalInMillis);
                }
            }
            return new TimeSerie(header.IntervalInMillis, list);
        }
      
        public async Task Save(string id, Occurrence occurrence) {
            await Save(id, new List<Occurrence> { occurrence });
        }

        public async Task Save(string id, List<Occurrence> occurrences) {
            var fullPath = _fileFinder.GetDataSetPath(id);
            var headerBytes = new byte[TimeSerieHeader.HEADER_SIZE];
            var lastDate = occurrences.Last().DateTime;
            try {
                using (var fs = _fileStreamOpener.OpenWithLock(fullPath)) {
                    await fs.ReadAsync(headerBytes, 0, headerBytes.Length);
                    var header = TimeSerieHeader.Deserialize(id, headerBytes);

                    var type = header.OcurrenceType;
                    using (var bw = new BinaryWriter(fs)) {
                        EnsureDateRange(bw, header, lastDate);
                        fs.Seek(
                            TimeSerieHeader.HEADER_SIZE +
                            (header.GetIndex(occurrences[0].DateTime)*type.GetBinarySize()), SeekOrigin.Begin);

                        for (int i = 0; i < occurrences.Count; i++) {
                            type.WriteToStream(bw, occurrences[i].Value);
                        }
                        if (header.Current < lastDate) {
                            header.Current = lastDate;
                        }
                        fs.Seek(0, SeekOrigin.Begin);
                        var headerInBytes = header.Serialize();
                        await fs.WriteAsync(headerInBytes, 0, headerInBytes.Length);
                    }
                }
            }
            catch (FileNotFoundException) when (DefaultTimeSerieHeader != null) {
                var header = new TimeSerieHeader(id, 
                                                 DefaultTimeSerieHeader.Start, 
                                                 DefaultTimeSerieHeader.End, 
                                                 DefaultTimeSerieHeader.IntervalInMillis, 
                                                 DefaultTimeSerieHeader.OcurrenceType, 
                                                 DefaultTimeSerieHeader.AutoExtendStep);
                await Create(header);
                await Save(id, occurrences);
            }
        }

        private static void EnsureDateRange(BinaryWriter bw, TimeSerieHeader timeSerieHeader, DateTime dateEnd) {
            if (dateEnd <= timeSerieHeader.End) {
                return;
            }
            var extendLimit = timeSerieHeader.IntervalInMillis * timeSerieHeader.AutoExtendStep;
            var maxDateAllowed = timeSerieHeader.End.Add(TimeSpan.FromMilliseconds(extendLimit));
            if (dateEnd > maxDateAllowed) {
                throw new DateAfterLimitException(timeSerieHeader, dateEnd);
            }
            ExtendFile(bw, timeSerieHeader);
        }

        private static void ExtendFile(BinaryWriter writer, TimeSerieHeader timeSerieHeader) {
            var nextDate = timeSerieHeader.End.Add(TimeSpan.FromMilliseconds(timeSerieHeader.IntervalInMillis));
            var list = new List<Occurrence>(timeSerieHeader.AutoExtendStep);
            for (int i = 0; i < timeSerieHeader.AutoExtendStep; i++) {
                list.Add(new Occurrence(nextDate.AddMilliseconds(timeSerieHeader.IntervalInMillis * i), null));
            }
            writer.Seek(0, SeekOrigin.End);
            for (int i = 0; i < list.Count; i++) {
                timeSerieHeader.OcurrenceType.WriteToStream(writer, list[i].Value);
            }
            timeSerieHeader.End = list.Last().DateTime;
        }
    }
}