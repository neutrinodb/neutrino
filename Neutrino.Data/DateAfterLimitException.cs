using System;
using Neutrino.Core;

namespace Neutrino.Data {
    public class DateAfterLimitException : Exception {
        public TimeSerieHeader Header { get; set; }
        public DateTime OccurrenceDate { get; set; }

        public DateAfterLimitException(TimeSerieHeader header, DateTime occurrenceDate) {
            Header = header;
            OccurrenceDate = occurrenceDate;
        }
    }
}