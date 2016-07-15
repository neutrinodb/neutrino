using System.Collections.Generic;
using System.Linq;

namespace Neutrino.Client {
    public class TimeSerieDouble {
        public int IntervalInMillis { get; }
        public List<TimeSerieDouble> Occurrences { get; }

        public TimeSerieDouble(int intervalInMillis, IEnumerable<TimeSerieDouble> occurrences) {
            IntervalInMillis = intervalInMillis;
            Occurrences = occurrences.ToList();
        }
    }
}