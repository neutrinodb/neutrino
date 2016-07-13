using System;

namespace Neutrino {
    public enum OcurrenceKind : byte {
        Decimal = 1,
        Double = 2,
        Int32 = 3
    }

    public static class OcurrenceKindExt {

        public static OcurrenceKind Parse(string label) {
            if(label.ToUpper() == "DECIMAL") return OcurrenceKind.Decimal;
            if(label.ToUpper() == "DOUBLE") return OcurrenceKind.Double;
            if(label.ToUpper() == "INT32") return OcurrenceKind.Int32;
            throw new ArgumentException("Can't support type : " + label, nameof(label));
        }

        //public static OcurrenceKindInfo GetInfo(this OcurrenceKind ocurrenceKind) {
        //    switch (ocurrenceKind) {
        //        case OcurrenceKind.Decimal:
        //            return new OccurrenceKindInfoForDecimal();
        //        case OcurrenceKind.Double:
        //            return new OccurrenceKindInfoForDouble();
        //        case OcurrenceKind.Float:
        //            return new OccurrenceKindInfoForFloat();
        //        case OcurrenceKind.Int16:
        //            return new OccurrenceKindInfoForInt16();
        //        case OcurrenceKind.Int32:
        //            return new OccurrenceKindInfoForInt32();
        //        case OcurrenceKind.Int64:
        //            return new OccurrenceKindInfoForInt64();
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(ocurrenceKind), ocurrenceKind, null);
        //    }
        //}
    }

    //public class OccurrenceKindInfoForDouble : OcurrenceKindInfo {
    //    public override object DefaultValue => Double.NaN;
    //    public override int OcurrenceSizeInBytes => sizeof(double);
    //}

    public abstract class OcurrenceKindInfo {
        public abstract object DefaultValue { get; }
        public abstract int OcurrenceSizeInBytes { get; }
    }
}