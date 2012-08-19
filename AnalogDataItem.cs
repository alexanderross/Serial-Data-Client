using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Data
{
    public class AnalogDataItem:MCUDataAsset
    {

        public Int32 MaxValue{get;set;} 
        public Int32 MinValue{get;set;}
        public Int32 ActualMax{get;set;}
        public Int32 ActualMin{get;set;}
        public string Units{get;set;}

        public AnalogDataItem(string name,Int32 val=0,Int16 max=1024, Int16 min=0, String unitsin="UNIT", Int16 actualMin = 0, Int16 actualMax = 1024)
            : base()
        {
           
            rawDataName = name;
            value = val;
        
            MaxValue = max;
            MinValue = min;
            Units = unitsin;
            ActualMax = actualMax;
            ActualMin = actualMin;

        }

        public int GetRealValue(){
            return (int)(((GetPct()) * (MaxValue - MinValue)) + MinValue);
        }

        public float GetPct()
        {
            return ((value - ActualMin) / (float)(ActualMax - ActualMin));
        }

        override public String FormatDataForSave()
        {
            String xmlString = "";
            xmlString += "<DATAINSTANCE type='ANALOG'>\r\n\t";
            xmlString += "<RAWNAME>";
            xmlString += rawDataName;
            xmlString += "</RAWNAME>\r\n\t";
            xmlString += "<REFINEDNAME>";
            xmlString += refinedDataName ;
            xmlString += "</REFINEDNAME>\r\n\t";
            xmlString += "<DESCRIPTION>";
            xmlString += dataDescription ;
            xmlString += "</DESCRIPTION>\r\n\t";
            xmlString += "<MINVALUE>";
            xmlString += MinValue.ToString() ;
            xmlString += "</MINVALUE>\r\n\t";
            xmlString += "<MAXVALUE>";
            xmlString += MaxValue.ToString() ;
            xmlString += "</MAXVALUE>\r\n\t";
            xmlString += "<UNITS>";
            xmlString += Units ;
            xmlString += "</UNITS>\r\n\t";
            xmlString += "<MINACTUALVALUE>";
            xmlString += ActualMin.ToString() ;
            xmlString += "</MINACTUALVALUE>\r\n\t";
            xmlString += "<MAXACTUALVALUE>";
            xmlString += ActualMax.ToString() ;
            xmlString += "</MAXACTUALVALUE>\r\n\t";
            xmlString += "</DATAINSTANCE>\r\n\t";

            return xmlString;

        }

        override public String GetType()
        {
            return "ANALOG";
        }

        override public String GetValueFormatted()
        {
            return GetRealValue().ToString();
        }

        override public String ToString()
        {
            return "RAW: " + rawDataName + ", REFINED: " + refinedDataName + ", DESCRIPTION: " + dataDescription + ", VAL: " + value;
        }

        override public String ToHTML()
        {
            return "<div class=\"sensorOutput\">" + ToSpecificHTML(MCUDataATTR.DATA_TYPE) + ToSpecificHTML(MCUDataATTR.DATA_ACTUAL_NAME) + ToSpecificHTML(MCUDataATTR.DATA_REFINED_NAME) + ToSpecificHTML(MCUDataATTR.DATA_DETAILS) + ToSpecificHTML(MCUDataATTR.DATA_ACTUALMIN) + ToSpecificHTML(MCUDataATTR.DATA_ACTUALMAX) + ToSpecificHTML(MCUDataATTR.DATA_MIN) + ToSpecificHTML(MCUDataATTR.DATA_MAX) + ToSpecificHTML(MCUDataATTR.DATA_UNITS) + ToSpecificHTML(MCUDataATTR.DATA_VALUE) + "</div><br/>";
        }

        public override string ToSpecificHTML(ushort param)
        {
            switch (param)
            {
                case MCUDataATTR.DATA_TYPE:
                    return "<b class=\"sensorType\">Analog</b>\n";
                case MCUDataATTR.DATA_ACTUAL_NAME:
                    return "Raw Name:</b><label class=\"sensorRaw\"> " + rawDataName + "</label>\n";
                case MCUDataATTR.DATA_REFINED_NAME:
                    return "<b class=\"sensorAttrName\">Refined Name:</b><label class=\"sensorRefined\"> " + refinedDataName + "</label>\n";
                case MCUDataATTR.DATA_DETAILS:
                    return "<b class=\"sensorAttrName\">Details:</b><label class=\"sensorDetails\"> " + dataDescription + "</label>\n";
                case MCUDataATTR.DATA_VALUE:
                    return "<b class=\"sensorAttrName\">Current Value:</b><label class=\"sensorVal\"> " + value.ToString() + "</label>\n";
                case MCUDataATTR.DATA_UNITS:
                    return "<b class=\"sensorAttrName\">Details:</b><label class=\"sensorUnits\"> " + Units + "</label>\n";
                case MCUDataATTR.DATA_MIN:
                    return "<b class=\"sensorAttrName\">Minumum Value:</b><label class=\"sensorMin\"> " + MinValue.ToString() + "</label>\n";
                case MCUDataATTR.DATA_MAX:
                    return "<b class=\"sensorAttrName\">Maximum Value:</b><label class=\"sensorMax\"> " + MaxValue.ToString() + "</label>\n";
                case MCUDataATTR.DATA_ACTUALMIN:
                    return "<b class=\"sensorAttrName\">Minumum Actual Value:</b><label class=\"sensorActualMin\"> " + ActualMin.ToString() + "</label>\n";
                case MCUDataATTR.DATA_ACTUALMAX:
                    return "<b class=\"sensorAttrName\">Maximum Actual Value:</b><label class=\"sensorActualMax\"> " + ActualMax.ToString() + "</label>\n";
                default:
                    return "<b class=\"error\">!Parameter not valid</b>\n";
            }
        }


    }
}
