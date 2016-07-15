using System;

namespace Neutrino.Client {
    public class OccurrenceDouble {
        public DateTime DateTime { get; set; }
        public double? Value { get; set; }

        public OccurrenceDouble(DateTime dateTime, double? value = null) {
            DateTime = dateTime;
            Value = value;
        }
    }
}