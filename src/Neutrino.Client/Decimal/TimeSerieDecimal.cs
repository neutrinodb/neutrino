using System.Collections.Generic;
using System.Linq;

namespace Neutrino.Client {
    public class TimeSerieDecimal {
        public int IntervalInMillis { get; }
        public List<OccurrenceDecimal> Occurrences { get; }

        public TimeSerieDecimal(int intervalInMillis, IEnumerable<OccurrenceDecimal> occurrences) {
            IntervalInMillis = intervalInMillis;
            Occurrences = occurrences.ToList();
        }
    }
}