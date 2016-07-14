using System;

namespace Neutrino {
    public class Occurrence : IEquatable<Occurrence> {

        public DateTime DateTime { get; set; }
        public object Value { get; set; }

        public Occurrence(DateTime dateTime, object value = null) {
            DateTime = dateTime;
            Value = value;
        }

        public bool Equals(Occurrence other) {
            return (other.DateTime == DateTime) && other.Value == Value;
        }
    }
}