﻿
using System;
using System.IO;

namespace Neutrino {
    public enum OccurrenceKind : byte {
        Decimal = 1,
        Double = 2
    }

    public static class OccurrenceKindExt {

        
        public static object ReadFromStream(this OccurrenceKind occurrenceKind, BinaryReader reader) {
            switch (occurrenceKind) {
                case OccurrenceKind.Decimal:
                    var vDecimal = reader.ReadDecimal();
                    return vDecimal == Decimal.MinValue ? (decimal?)null : vDecimal;
                case OccurrenceKind.Double:
                    var vDouble = reader.ReadDouble();
                    return vDouble == Double.NaN? (double?)null : vDouble;
            }
            throw new ArgumentException(occurrenceKind + " is not supported", nameof(occurrenceKind));
        }

        public static void WriteDefaultToStream(this OccurrenceKind occurrenceKind, BinaryWriter writer) {
            switch (occurrenceKind) {
                case OccurrenceKind.Decimal:
                    writer.Write(decimal.MinValue);
                    return;
                case OccurrenceKind.Double:
                    writer.Write(double.NaN);
                    return;
            }
            throw new ArgumentException(occurrenceKind + " is not supported", nameof(occurrenceKind));
        }

        public static int GetBinarySize(this OccurrenceKind occurrenceKind) {
            switch (occurrenceKind) {
                case OccurrenceKind.Decimal:
                    return sizeof(decimal);
                case OccurrenceKind.Double:
                    return sizeof(double);
            }
            throw new ArgumentException(occurrenceKind + " is not supported", nameof(occurrenceKind));
        }

        public static object GetDefault(this OccurrenceKind occurrenceKind) {
            switch (occurrenceKind) {
                case OccurrenceKind.Decimal:
                    return decimal.MinValue;
                case OccurrenceKind.Double:
                    return double.NaN;
            }
            throw new ArgumentException(occurrenceKind + " is not supported", nameof(occurrenceKind));
        }
    }
}