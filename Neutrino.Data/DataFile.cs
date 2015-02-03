using System;
using System.IO;
using System.Threading.Tasks;
using Neutrino.Core;

namespace Neutrino.Data {
    public class DataFile {
        private const int HeaderSize = (sizeof (Int64) * 3) + sizeof(Int32);

        public async Task<TimeSerieInfo> ReadHeader(string id, string path) {
            var header = new byte[HeaderSize];
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                await fs.ReadAsync(header, 0, HeaderSize);
            }
            using (var br = new BinaryReader(new MemoryStream(header))) {
                var start = new DateTime(br.ReadInt64());
                var end = new DateTime(br.ReadInt64());
                var current = new DateTime(br.ReadInt64());
                var interval = br.ReadInt32();
                var autoExtendStep = br.ReadInt32();
                var ts = new TimeSerieInfo(id, start, end, interval, autoExtendStep) {
                    Current = current
                };
                return ts;
            }
        }

        public static byte[] SerializeTimeSerieInfo(TimeSerieInfo timeSerieInfo) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                bw.Write(timeSerieInfo.Start.ToBinary());
                bw.Write(timeSerieInfo.End.ToBinary());
                bw.Write(timeSerieInfo.Current.ToBinary());
                bw.Write(timeSerieInfo.IntervalInMillis);
            }
            return ms.ToArray();
        }
        public static byte[] EmptyTimeSerieBodyToBytes(TimeSerieInfo timeSerieInfo) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                for (int i = 0; i < timeSerieInfo.TotalLength; i++) {
                    bw.Write(Decimal.MinValue);
                }
            }
            return ms.ToArray();
        }

        public static Task ExtendFile(TimeSerieInfo ts, DateTime dateEnd) {
            return null;
        }
    }
}