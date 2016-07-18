using System;

namespace Neutrino {
    public class InvalidIdException : Exception {
        public string InvalidId { get; }

        public InvalidIdException(Exception exeption, string invalidId) : base("Invalid Id", exeption) {
            InvalidId = invalidId;
        }
    }
}