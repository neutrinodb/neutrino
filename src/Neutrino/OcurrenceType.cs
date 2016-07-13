using System;
using System.Net.Http.Headers;

namespace Neutrino {
    

    public class OccurrenceKindInfoForDouble : OcurrenceKindInfo {
        public override object DefaultValue => Double.NaN;
        public override int OcurrenceSizeInBytes => sizeof(double);
    }

    public class OccurrenceKindInfoForDecimal : OcurrenceKindInfo {
        public override object DefaultValue => Decimal.MinValue;
        public override int OcurrenceSizeInBytes => sizeof(decimal);
    }

    public class OccurrenceKindInfoForFloat : OcurrenceKindInfo {
        public override object DefaultValue => float.NaN;
        public override int OcurrenceSizeInBytes => sizeof(float);
    }

    public class OccurrenceKindInfoForInt16 : OcurrenceKindInfo {
        public override object DefaultValue => Int16.MinValue;
        public override int OcurrenceSizeInBytes => sizeof(Int16);
    }

    public class OccurrenceKindInfoForInt32 : OcurrenceKindInfo {
        public override object DefaultValue => Int32.MinValue;
        public override int OcurrenceSizeInBytes => sizeof(Int32);
    }

    public class OccurrenceKindInfoForInt64 : OcurrenceKindInfo {
        public override object DefaultValue => Int64.MinValue;
        public override int OcurrenceSizeInBytes => sizeof(Int64);
    }

    
}