using System;

namespace Neutrino.Client {
    public class OccurrenceDecimal {
        public DateTime DateTime { get; set; }
        public decimal? Value { get; set; }

        public OccurrenceDecimal(DateTime dateTime, decimal? value = null) {
            DateTime = dateTime;
            Value = value;
        }
    }
}