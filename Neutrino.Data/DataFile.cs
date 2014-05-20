using System;
using System.IO;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public static class DataFile {
        private const int HeaderSize = (sizeof (Int64) * 3) + sizeof(Int32);

        public static async Task<TimeSerie> WalkStreamExtractingTimeSerieHeader(string id, Stream stream) {
            var header = new byte[HeaderSize];
            await stream.ReadAsync(header, 0, HeaderSize);
            using (var br = new BinaryReader(new MemoryStream(header))) {
                var start = new DateTime(br.ReadInt64());
                var end = new DateTime(br.ReadInt64());
                var current = new DateTime(br.ReadInt64());
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