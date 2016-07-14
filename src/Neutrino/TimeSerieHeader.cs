using System;
using System.IO;

namespace Neutrino {
    public class TimeSerieHeader {
        public string Id { get; private set; }
        public OccurrenceKind OcurrenceType { get; }
        public DateTime Start { get; }
        public DateTime End { get; set; }
        public DateTime Current { get; set; }
        public int IntervalInMillis { get; }
        public int AutoExtendStep { get; }
        public long TotalLength => GetIndex(End) + 1;

        public const int HEADER_SIZE = sizeof(byte) + (sizeof(Int32) * 2) + (sizeof(Int64)*3);
        public TimeSerieHeader(string id, DateTime start, DateTime end, int intervalInMillis, OccurrenceKind ocurrenceType = OccurrenceKind.Decimal, int autoExtendStep = -1) {
            Id = id;
            OcurrenceType = ocurrenceType;
            Start = start;
            End = end;
            IntervalInMillis = intervalInMillis;
            Current = Start;
            AutoExtendStep = autoExtendStep < 0 ? 1000 : autoExtendStep;
            //todo: check if end is valid according to start and interval
        }

        public long GetIndex(DateTime date) {
            if ((date < Start) || (date > End)) {
                return -1;
            }
            TimeSpan ts = date - Start;
            return Convert.ToInt32(ts.TotalMilliseconds / IntervalInMillis);
        }

        public long CalcNumberOfRegisters(DateTime start, DateTime end) {
            return (long)(end - start).TotalMilliseconds / IntervalInMillis + 1;
        }

        public byte[] Serialize() {
            using (var ms = new MemoryStream()) {
                using (var bw = new BinaryWriter(ms)) {
                    bw.Write((byte)OcurrenceType);
                    bw.Write(Start.ToBinary());
                    bw.Write(End.ToBinary());
                    bw.Write(Current.ToBinary());
                    bw.Write(IntervalInMillis);
                    bw.Write(AutoExtendStep);
                }
                return ms.ToArray();
            }
        }

        public static TimeSerieHeader Deserialize(string id, Stream stream) {
            TimeSerieHeader ts;
            using (var br = new BinaryReader(stream)) {
                var ocurrenceType = (OccurrenceKind)br.Read();
                var start = DateTime.FromBinary(br.ReadInt64());
                var end = DateTime.FromBinary(br.ReadInt64());
                var current = DateTime.FromBinary(br.ReadInt64());
                var interval = br.ReadInt32();
                var autoExtendStep = br.ReadInt32();
                ts = new TimeSerieHeader(id, start, end, interval, ocurrenceType, autoExtendStep) {
                    Current = current
                };
            }
            return ts;
        }

        public static TimeSerieHeader Deserialize(string id, byte[] bytes) {
            using (var ms = new MemoryStream(bytes)) {
                return Deserialize(id, ms);
            }
        }
    }
}