using System.Collections.Generic;

namespace Neutrino {
    public class TimeSerie<T> {
        public int IntervalInMillis { get; private set; }
        public List<Occurrence<T>> Occurrences { get; private set; }

        public TimeSerie(int intervalInMillis) : this(intervalInMillis, new List<Occurrence<T>>()) {
        }

        public TimeSerie(int intervalInMillis, List<Occurrence<T>> occurrences) {
            IntervalInMillis = intervalInMillis;
            Occurrences = occurrences;
        }
    }
}