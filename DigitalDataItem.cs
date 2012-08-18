using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Data
{
    public class DigitalDataItem:MCUDataAsset
    {

        public DigitalDataItem(String rawName,UInt32 valueIn=0):base()
        {
            rawDataName = rawName;
            value = valueIn;
        }

        override public String FormatDataForSave()
        {
            String xmlString = "";
            xmlString += "<DATAINSTANCE type='DIGITAL'>\r\n\t";
            xmlString += "<RAWNAME>";
            xmlString += rawDataName;
            xmlString += "</RAWNAME>\r\n\t";
            xmlString += "<REFINEDNAME>";
            xmlString += refinedDataName;
            xmlString += "</REFINEDNAME>\r\n\t";
            xmlString += "<DESCRIPTION>";
            xmlString += dataDescription;
            xmlString += "</DESCRIPTION>\r\n\t";
            xmlString += "</DATAINSTANCE>\r\n\t";

            return xmlString;

        }

        override public String GetType()
        {
            return "DIGITAL";
        }

        override public String GetValueFormatted()
        {
            return value == 1 ? "HIGH" : "LOW";
        }

        override public String ToString()
        {
            return "RAW: " + rawDataName + ", REFINED: " + refinedDataName + ", DESCRIPTION: " + dataDescription + ", VAL: " + value;
        }

        override public String ToHTML()
        {
            return "<div class=\"sensorOutput\">" + ToSpecificHTML(MCUDataATTR.DATA_TYPE) + ToSpecificHTML(MCUDataATTR.DATA_ACTUAL_NAME) + ToSpecificHTML(MCUDataATTR.DATA_REFINED_NAME) + ToSpecificHTML(MCUDataATTR.DATA_DETAILS) + ToSpecificHTML(MCUDataATTR.DATA_VALUE) + "</div>";
        }

        public override string ToSpecificHTML(ushort param)
        {
            switch (param)
            {
                case MCUDataATTR.DATA_TYPE:
                    return "<b class=\"sensorType\">Digital</b>";
                case MCUDataATTR.DATA_ACTUAL_NAME:
                    return "<b class=\"sensorAttrName\">Raw Name:</b><label class=\"sensorRaw\"> " + rawDataName + "</label>";
                case MCUDataATTR.DATA_REFINED_NAME:
                    return "<b class=\"sensorAttrName\">Refined Name:</b><label class=\"sensorRefined\"> " + refinedDataName + "</label>";
                case MCUDataATTR.DATA_DETAILS:
                    return "<b class=\"sensorAttrName\">Details:</b><label class=\"sensorDetails\"> " + dataDescription + "</label>";
                case MCUDataATTR.DATA_VALUE:
                    return "<b class=\"sensorAttrName\">Current Value:</b><label class=\"sensorVal\"> " + GetValueFormatted() + "</label>";
                default:
                    return "<b class=\"error\">!Parameter not valid</b>";
            }
        }
    }
}
