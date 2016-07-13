using System;
using System.Collections.Generic;
using System.IO;

namespace Neutrino {
    public abstract class Occurrence<T> : IEquatable<Occurrence<T>> {

        public DateTime DateTime { get; set; }
        public T Value { get; set; }

        protected Occurrence(DateTime dateTime, T value) {
            DateTime = dateTime;
            Value = value;
        }

        //public abstract void WriteValueToStream(StreamWriter stream);

        public bool Equals(Occurrence<T> other) {
            return (other.DateTime == DateTime) && 
                EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
    }
}