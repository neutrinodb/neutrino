using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data {
    public class DataFile {
        private readonly IFileFinder _fileFinder;
        public static readonly int HeaderSize = (sizeof (Int64) * 3) + sizeof(Int32) * 2;
        public static readonly int RegisterSize = sizeof(decimal);

        public DataFile(IFileFinder fileFinder) {
            _fileFinder = fileFinder;
        }

        public async Task<byte[]> ReadHeaderAsync(string id) {
            var path = _fileFinder.GetDataSetPath(id);
            var header = new byte[HeaderSize];
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                await fs.ReadAsync(header, 0, HeaderSize);
            }
            return header;
        }

        public async Task<TimeSerie> ReadRegistersAsync(TimeSerieHeader header, DateTime start, long numberOfRegisters) {
            var path = _fileFinder.GetDataSetPath(header.Id);
            var data = new byte[numberOfRegisters * RegisterSize];
            long indexStart = GetIndexStart(start, header);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                fs.Seek(indexStart, SeekOrigin.Current);
                await fs.ReadAsync(data, 0, data.Length);
            }
            return DeserializeRegisters(data, start, numberOfRegisters, header.IntervalInMillis);
        }

        private long GetIndexStart(DateTime start, TimeSerieHeader ts) {
            return HeaderSize + ts.GetIndex(start) * RegisterSize;
        }

        private static TimeSerie DeserializeRegisters(byte[] data, DateTime start, long numberOfRegisters, int intervalInMillis) {
            var list = new List<Occurrence>();
            using (var br = new BinaryReader(new MemoryStream(data))) {
                var current = start;
                for (int i = 0; i < numberOfRegisters; i++) {
                    current = current.AddMilliseconds(intervalInMillis);
                    var oc = new Occurrence();
                    oc.DateTime = current;
                    oc.Value = br.ReadDecimal();
                    list.Add(oc);
                }
            }
            return new TimeSerie(intervalInMillis, list);
        }

        public static byte[] SerializeTimeSerieHeader(TimeSerieHeader timeSerieHeader) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                bw.Write(timeSerieHeader.Start.ToBinary());
                bw.Write(timeSerieHeader.End.ToBinary());
                bw.Write(timeSerieHeader.Current.ToBinary());
                bw.Write(timeSerieHeader.IntervalInMillis);
                bw.Write(timeSerieHeader.AutoExtendStep);
            }
            return ms.ToArray();
        }

        public static byte[] SerializeEmptyTimeSerieBody(TimeSerieHeader timeSerieHeader) {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms)) {
                for (int i = 0; i < timeSerieHeader.TotalLength; i++) {
                    bw.Write(Decimal.MinValue);
                }
            }
            return ms.ToArray();
        }

        public async Task ExtendFileAsync(TimeSerieHeader timeSerieHeader, DateTime dateEnd) {
            var nextDate = timeSerieHeader.End.Add(TimeSpan.FromMilliseconds(timeSerieHeader.IntervalInMillis));
            var list = new List<Occurrence>(timeSerieHeader.AutoExtendStep);
            for (int i = 0; i < timeSerieHeader.AutoExtendStep; i++) {
                list.Add(new Occurrence(nextDate.AddMilliseconds(timeSerieHeader.IntervalInMillis * i), null));
            }
            timeSerieHeader.End = list.Last().DateTime;
            await WriteRegistersAsync(timeSerieHeader, list);
        }

        public async Task CreateAsync(TimeSerieHeader timeSerieHeader) {
            var path = _fileFinder.GetDataSetPath(timeSerieHeader.Id);
            using (var fs = new FileStream(path, FileMode.CreateNew)) {
                var header = DataFile.SerializeTimeSerieHeader(timeSerieHeader);
                await fs.WriteAsync(header, 0, header.Length);
                var body = SerializeEmptyTimeSerieBody(timeSerieHeader);
                await fs.WriteAsync(body, 0, body.Length);
            }
        }

        public async Task WriteRegistersAsync(TimeSerieHeader timeSerieHeader, List<Occurrence> occurrences) {
            var path = _fileFinder.GetDataSetPath(timeSerieHeader.Id);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
                long indexStart = GetIndexStart(occurrences[0].DateTime, timeSerieHeader);
                fs.Seek(indexStart, SeekOrigin.Begin);
                var bytes = new byte[occurrences.Count * RegisterSize];
                for (int i = 0; i < occurrences.Count; i++) {
                    decimal value = !occurrences[i].Value.HasValue ? Decimal.MinValue : occurrences[i].Value.Value;
                    Buffer.BlockCopy(Decimal.GetBits(value), 0, bytes, i * RegisterSize, 16);
                }
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}