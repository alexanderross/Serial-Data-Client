using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Data
{
    static class DataDisplayFactory
    {

        public static MCUDataDisplay GetInstance(MCUDataAsset input)
        {
            if (input.GetType().Equals("DIGITAL"))
            {
                return new DigitalDataDisplay(input);
            }
            else
            {
                return new AnalogDataDisplay(input);
            }
        }
    }
}
