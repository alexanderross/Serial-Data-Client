using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Data
{

    public abstract class MCUDataAsset
    {
        public string rawDataName
        {
            get;
            set;
        }
        public string refinedDataName
        {
            get;
            set;
        }
        public string dataDescription
        {
            get;
            set;
        }
        public Int32 value
        {
            get;
            set;
        }

        public abstract String FormatDataForSave();
        public abstract String GetType();
        public abstract String GetValueFormatted();
        public abstract String ToString();
        public abstract String ToHTML();
        public abstract String ToSpecificHTML(UInt16 param); 
    }

    static public class MCUDataATTR
    {
        public const UInt16 DATA_DETAILS =0;
        public const UInt16 DATA_REFINED_NAME=1;
        public const UInt16 DATA_VALUE = 2;
        public const UInt16 DATA_MIN = 3;
        public const UInt16 DATA_MAX = 4;
        public const UInt16 DATA_UNITS = 5;
        public const UInt16 DATA_ACTUALMIN = 6;
        public const UInt16 DATA_ACTUALMAX = 7;
        public const UInt16 DATA_ACTUAL_NAME = 8;
        public const UInt16 DATA_TYPE = 9;
    }
}
