using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CCTVClient.Data
{
    static class MCUAssetFactory
    {
        public static MCUDataAsset getInstance(KeyValuePair<String, int> input)
        {
            if (input.Key.Contains('&'))
            {
                return new DigitalDataItem(input.Key.Replace("&",""),(Int32)input.Value);
            }
            else
            {
                return new AnalogDataItem(input.Key.Replace("@", ""), (Int32)input.Value);
            }
        }

        public static MCUDataAsset getInstance(String type,String name)
        {
            if (type.Equals("DIGITAL"))
            {
                return new DigitalDataItem(name,0);
            }
            else
            {
                return new AnalogDataItem(name, 0);
            }
        }
    }
}
