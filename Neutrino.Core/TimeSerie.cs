using System;

namespace Neutrino.Core {
    public class TimeSerie {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public DateTime Current { get; set; }
        public int IntervalInMillisInMillis { get; private set; }
        public string Id { get; private set; }

        public long TotalLength {
            get { return GetIndex(End) + 1; }
        }

        public TimeSerie(string id, DateTime start, DateTime end, int intervalInMillis) {
            Id = id;
            Start = start;
            End = end;
            IntervalInMillisInMillis = intervalInMillis;
            Current = Start;
            //todo: check if end is valid according to start and interval
        }

        public long GetIndex(DateTime date) {
            if ((date < Start) || (date > End)) {
                return -1;
            }
            TimeSpan ts = date - Start;
            return Convert.ToInt32(ts.TotalMilliseconds/IntervalInMillisInMillis);
        }
    }
}