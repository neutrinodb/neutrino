using System;
using System.IO;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public static class DataFile {
        private const int HeaderSize = (sizeof (Int64) * 3) + sizeof(Int32);

        public static async Task<TimeSerieInfo> WalkStreamExtractingTimeSerieHeader(string id, Stream stream) {
            var header = new byte[HeaderSize];
            await stream.ReadAsync(header, 0, HeaderSize);
            using (var br = new BinaryReader(new MemoryStream(header))) {
                var start = new DateTime(br.ReadInt64());
                var end = new DateTime(br.ReadInt64());
                var current = new DateTime(br.ReadInt64());
                var interval = br.ReadInt32();
                var ts = new TimeSerieInfo(id, start, end, interval) {
                    Current = current
                };
                return ts;
            }
        }

        public static byte[] TimeSerieHeaderToBytes(TimeSerieInfo timeSerieInfo) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                bw.Write(timeSerieInfo.Start.ToBinary());
                bw.Write(timeSerieInfo.End.ToBinary());
                bw.Write(timeSerieInfo.Current.ToBinary());
                bw.Write(timeSerieInfo.IntervalInMillis);
            }
            return ms.ToArray();
        }
        public static byte[] TimeSerieBodyToBytes(TimeSerieInfo timeSerieInfo) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                for (int i = 0; i < timeSerieInfo.TotalLength; i++) {
                    bw.Write(Decimal.MinValue);
                }
            }
            return ms.ToArray();
        }
    }
}