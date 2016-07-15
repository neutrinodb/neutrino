using System;

namespace Neutrino.Client {
    public class TimeSerieHeader {
        public string Id { get; private set; }
        public OccurrenceKind OcurrenceType { get; }
        public DateTime Start { get; }
        public DateTime End { get; set; }
        public DateTime Current { get; set; }
        public int IntervalInMillis { get; }
        public int AutoExtendStep { get; }

        public TimeSerieHeader(string id, DateTime start, DateTime end, int intervalInMillis, OccurrenceKind ocurrenceType = OccurrenceKind.Decimal, int autoExtendStep = -1) {
            Id = id;
            OcurrenceType = ocurrenceType == 0 ? OccurrenceKind.Decimal : ocurrenceType;
            Start = start;
            End = end;
            IntervalInMillis = intervalInMillis;
            Current = Start;
            AutoExtendStep = autoExtendStep < 0 ? 1000 : autoExtendStep;
        }
    }
}