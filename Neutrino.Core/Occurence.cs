using System;

namespace Neutrino.Core {
    public class Occurence : IEquatable<Occurence> {

        public DateTime DateTime { get; set; }
        public decimal? Value { get; set; }

        public bool Equals(Occurence other) {
            return other.DateTime == DateTime && other.Value == Value;
        }
    }
}