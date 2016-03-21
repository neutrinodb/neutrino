using System.Collections.Generic;

namespace Neutrino {
    public class TimeSerie {
        public int IntervalInMillis { get; private set; }
        public List<Occurrence> Occurrences { get; private set; }

        public TimeSerie(int intervalInMillis, List<Occurrence> occurrences) {
            IntervalInMillis = intervalInMillis;
            Occurrences = occurrences;
        }
    }
}