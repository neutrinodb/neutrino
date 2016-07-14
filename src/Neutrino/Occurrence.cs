using System;

namespace Neutrino {
    public class Occurrence {

        public DateTime DateTime { get; set; }
        public object Value { get; set; }

        public Occurrence(DateTime dateTime, object value = null) {
            DateTime = dateTime;
            Value = value;
        }

        public override int GetHashCode() {
            unchecked {
                var hash = 17;
                hash *= 23 + DateTime.GetHashCode();
                hash *= 23 + Value.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj) {
            if (!(obj is Occurrence)) {
                return false;
            }
            return GetHashCode() == obj.GetHashCode();
        }
    }
}