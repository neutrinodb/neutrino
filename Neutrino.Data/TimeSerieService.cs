using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public class TimeSerieService : ITimeSerieService {
        private IFileFinder _fileFinder;

        public TimeSerieService(IFileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public async Task<string> Create(TimeSerieHeader timeSerieHeader) {
            var fullPath = _fileFinder.GetDataSetPath(timeSerieHeader.Id);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            var dataFile = new DataFile(_fileFinder);
            await dataFile.CreateAsync(timeSerieHeader);
            return fullPath;
        }

        public async Task<TimeSerie> List(string id, DateTime start, DateTime end) {
            var dataFile = new DataFile(_fileFinder);
            var headerBytes = await dataFile.ReadHeaderAsync(id);
            var timeSerieHeader = DeserializeHeader(id, headerBytes);
            long numberOfRegisters = CalcNumberOfRegisters(start, end, timeSerieHeader.IntervalInMillis);
            return await dataFile.ReadRegistersAsync(timeSerieHeader, start, numberOfRegisters);
        }

        private static TimeSerieHeader DeserializeHeader(string id, byte[] bytes) {
            TimeSerieHeader ts;
            using (var br = new BinaryReader(new MemoryStream(bytes))) {
                var start = new DateTime(br.ReadInt64());
                var end = new DateTime(br.ReadInt64());
                var current = new DateTime(br.ReadInt64());
                var interval = br.ReadInt32();
                var autoExtendStep = br.ReadInt32();
                ts = new TimeSerieHeader(id, start, end, interval, autoExtendStep) {
                    Current = current
                };
            }
            return ts;
        }

        private static long CalcNumberOfRegisters(DateTime start, DateTime end, long intervalInMillis) {
            return ((long) (end - start).TotalMilliseconds / intervalInMillis) + 1;
        }

        public async Task Add(string id, Occurrence occurrence) {
            await Add(id, new List<Occurrence> { occurrence });
        }

        public async Task Add(string id, List<Occurrence> occurrences) {
            var dataFile = new DataFile(_fileFinder);
            var headerBytes = await dataFile.ReadHeaderAsync(id);
            var timeSerieHeader = DeserializeHeader(id, headerBytes);

            var dateEnd = occurrences.Last().DateTime;
            await EnsureDateRange(timeSerieHeader, dateEnd, dataFile);
            await dataFile.WriteRegistersAsync(timeSerieHeader, occurrences);
        }

        private static async Task EnsureDateRange(TimeSerieHeader timeSerieHeader, DateTime dateEnd, DataFile dataFile) {
            if (dateEnd <= timeSerieHeader.End) {
                return;
            }
            var extendLimit = timeSerieHeader.IntervalInMillis * timeSerieHeader.AutoExtendStep;
            var maxDateAllowed = timeSerieHeader.End.Add(TimeSpan.FromMilliseconds(extendLimit));
            if (dateEnd > maxDateAllowed) {
                throw new DateAfterLimitException(timeSerieHeader, dateEnd);
            }
            await dataFile.ExtendFileAsync(timeSerieHeader, dateEnd);
        }
    }
}