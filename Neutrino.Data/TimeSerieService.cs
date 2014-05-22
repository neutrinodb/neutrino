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
                var header = DataFile.TimeSerieHeaderToBytes(timeSerieInfo);
                await fs.WriteAsync(header, 0, header.Length);

                var body = DataFile.TimeSerieBodyToBytes(timeSerieInfo);
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
            var path = _fileFinder.GetDataSetPath(id);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
                TimeSerieInfo ts = await DataFile.WalkStreamExtractingTimeSerieHeader(id, fs);
                long indexStart = ts.GetIndex(occurrence.DateTime)*RegisterSize;
                fs.Seek(indexStart, SeekOrigin.Current);
                decimal value = !occurrence.Value.HasValue ? Decimal.MinValue : occurrence.Value.Value;
                var bytes = new byte[16];
                Buffer.BlockCopy(decimal.GetBits(value), 0, bytes, 0, 16);
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}