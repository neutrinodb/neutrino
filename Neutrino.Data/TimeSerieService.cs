using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public class TimeSerieService : ITimeSerieService {
        private IFileFinder _fileFinder;

        public TimeSerieService(IFileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public async Task<string> Create(TimeSerie timeSerie) {
            var path = _fileFinder.GetDataSetPath(timeSerie.Id);
            using (var fs = new FileStream(path, FileMode.CreateNew)) {
                var header = DataFile.TimeSerieHeaderToBytes(timeSerie);
                await fs.WriteAsync(header, 0, header.Length);

                var body = DataFile.TimeSerieBodyToBytes(timeSerie);
                await fs.WriteAsync(body, 0, body.Length);
                return path;
            }
        }

        public async Task<List<Occurrence>> List(string id, DateTime start, DateTime end) {
            var path = _fileFinder.GetDataSetPath(id);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                TimeSerie ts = await DataFile.WalkStreamExtractingTimeSerieHeader(id, fs);
                long indexStart = ts.GetIndex(start)*sizeof (Int64);
                fs.Seek(indexStart, SeekOrigin.Current);
                long num = ((end - start).Ticks / ts.IntervalInMillisInMillis / 10000) + 1;

                var data = new byte[num * sizeof(decimal)];
                await fs.ReadAsync(data, 0, (int) num);

                using (var br = new BinaryReader(new MemoryStream(data))) {
                    DateTime current = start;
                    var list = new List<Occurrence>();
                    for (int i = 0; i < num; i++) {
                        current = current.AddMilliseconds(ts.IntervalInMillisInMillis*i);
                        var oc = new Occurrence();
                        oc.DateTime = current;
                        oc.Value = br.ReadDecimal();
                        list.Add(oc);
                    }
                    return list;
                }
            }
        }
    }
}