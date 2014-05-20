using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Neutrino.Core {
    public class TimeSerieService {
        private IFileFinder _fileFinder;

        public TimeSerieService(IFileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public async Task Create(TimeSerie timeSerie) {
            string path = _fileFinder.GetDataSetPath(timeSerie.Id);
            using (var fs = new FileStream(path, FileMode.CreateNew)) {
                var header = DataFile.TimeSerieHeaderToBytes(timeSerie);
                await fs.WriteAsync(header, 0, header.Length);

                var body = DataFile.TimeSerieBodyToBytes(timeSerie);
                await fs.WriteAsync(body, 0, header.Length);
            }
        }

        public async Task<List<Occurence>> List(string id, DateTime start, DateTime end) {
            string path = _fileFinder.GetDataSetPath(id);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                TimeSerie ts = await DataFile.WalkStreamExtractingTimeSerieHeader(id, fs);
                long indexStart = ts.GetIndex(start);
                fs.Seek(indexStart, SeekOrigin.Current);
                long num = (end - start).Ticks / ts.IntervalInMillisInMillis;

                var data = new byte[num * sizeof(decimal)];
                await fs.ReadAsync(data, 0, (int) num);

                using (var br = new BinaryReader(new MemoryStream(data))) {
                    DateTime current = start;
                    var list = new List<Occurence>();
                    for (int i = 0; i < num; i++) {
                        current = current.AddMilliseconds(ts.IntervalInMillisInMillis*i);
                        var oc = new Occurence();
                        oc.DateTime = current;
                        oc.Value = br.ReadDecimal();
                        list.Add(oc);
                    }
                    return list;
                }
            }
        }
    }

    public static class DataFile {
        private const int HeaderSize = (sizeof (Int64) * 3) + sizeof(Int32);

        public static async Task<TimeSerie> WalkStreamExtractingTimeSerieHeader(string id, Stream stream) {
            var header = new byte[HeaderSize];
            await stream.ReadAsync(header, 0, HeaderSize);
            using (var br = new BinaryReader(new MemoryStream(header))) {
                var start = new DateTime(br.ReadInt32());
                var end = new DateTime(br.ReadInt32());
                var current = new DateTime(br.ReadInt32());
                var interval = br.ReadInt32();
                var ts = new TimeSerie(id, start, end, interval) {
                    Current = current
                };
                return ts;
            }
        }

        public static byte[] TimeSerieHeaderToBytes(TimeSerie timeSerie) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                bw.Write(timeSerie.Start.Ticks);
                bw.Write(timeSerie.End.Ticks);
                bw.Write(timeSerie.Current.Ticks);
                bw.Write(timeSerie.IntervalInMillisInMillis);
            }
            return ms.ToArray();
        }
        public static byte[] TimeSerieBodyToBytes(TimeSerie timeSerie) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                for (int i = 0; i < timeSerie.TotalLength; i++) {
                    bw.Write(Decimal.MinValue);
                }
            }
            return ms.ToArray();
        }
    }
}