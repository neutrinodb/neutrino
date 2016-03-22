using System;

namespace Neutrino.Data {
    public class HeaderCorruptedException : Exception {

        public string TimeSerieId { get; private set; }

        public HeaderCorruptedException(string timeSerieId) {
            TimeSerieId = timeSerieId;
        }

        public HeaderCorruptedException(string timeSerieId, string message) : base(message) {
            TimeSerieId = timeSerieId;
        }

        public HeaderCorruptedException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}