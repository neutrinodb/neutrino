using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public class TimeSerieService : ITimeSerieService {
        private IFileFinder _fileFinder;
        private const int RegisterSize = sizeof(decimal);

        public TimeSerieService(IFileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public async Task<string> Create(TimeSerieInfo timeSerieInfo) {
            var path = _fileFinder.GetDataSetPath(timeSerieInfo.Id);
            using (var fs = new FileStream(path, FileMode.CreateNew)) {
                var header = DataFile.SerializeTimeSerieInfo(timeSerieInfo);
                await fs.WriteAsync(header, 0, header.Length);

                var body = DataFile.EmptyTimeSerieBodyToBytes(timeSerieInfo);
                await fs.WriteAsync(body, 0, body.Length);
                return path;
            }
        }

        public async Task<TimeSerie> List(string id, DateTime start, DateTime end) {
            var path = _fileFinder.GetDataSetPath(id);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                TimeSerieInfo ts = await DataFile.WalkStreamExtractingTimeSerieHeader(id, fs);
                long indexStart = ts.GetIndex(start) * RegisterSize;
                fs.Seek(indexStart, SeekOrigin.Current);
                long num = ((long)(end - start).TotalMilliseconds / ts.IntervalInMillis) + 1;

                var data = new byte[num * RegisterSize];
                await fs.ReadAsync(data, 0, data.Length);

                using (var br = new BinaryReader(new MemoryStream(data))) {
                    DateTime current = start;
                    var list = new List<Occurrence>();
                    for (int i = 0; i < num; i++) {
                        current = current.AddMilliseconds(ts.IntervalInMillis);
                        var oc = new Occurrence();
                        oc.DateTime = current;
                        oc.Value = br.ReadDecimal();
                        list.Add(oc);
                    }
                    return new TimeSerie(ts.IntervalInMillis, list);
                }
            }
        }

        public async Task Add(string id, Occurrence occurrence) {
            await Add(id, new List<Occurrence> {occurrence});
        }

        public async Task Add(string id, List<Occurrence> occurrences) {
            var path = _fileFinder.GetDataSetPath(id);
            DataFile.ReadHeader(id, path);
            TimeSerieInfo ts;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
                ts = await DataFile.WalkStreamExtractingTimeSerieHeader(id, fs);
            }
            var dateEnd = occurrences.Last().DateTime;
            if (dateEnd > ts.End) {
                await DataFile.ExtendFile(ts, dateEnd);
            }
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
                long indexStart = ts.GetIndex(occurrences[0].DateTime)*RegisterSize;
                fs.Seek(indexStart, SeekOrigin.Current);
                var bytes = new byte[occurrences.Count*RegisterSize];
                for (int i = 0; i < occurrences.Count; i++) {
                    decimal value = !occurrences[i].Value.HasValue ? Decimal.MinValue : occurrences[i].Value.Value;
                    Buffer.BlockCopy(Decimal.GetBits(value), 0, bytes, i*RegisterSize, 16);
                }
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}