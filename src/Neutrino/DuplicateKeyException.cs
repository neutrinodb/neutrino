using System;

namespace Neutrino {
    public class DuplicateKeyException : Exception {
        public DuplicateKeyException(string existingId) {
            Id = existingId;
        }

        public string Id { get; }
    }
}