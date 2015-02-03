using System;

namespace Neutrino.Core {
    public class TimeSerieInfo {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public DateTime Current { get; set; }
        public int IntervalInMillis { get; private set; }
        public string Id { get; private set; }

        public int AutoExtendStep { get; private set; }

        public long TotalLength {
            get { return GetIndex(End) + 1; }
        }

        public TimeSerieInfo(string id, DateTime start, DateTime end, int intervalInMillis, int autoExtendStep = -1) {
            Id = id;
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
            return Convert.ToInt32(ts.TotalMilliseconds/IntervalInMillis);
        }
    }
}