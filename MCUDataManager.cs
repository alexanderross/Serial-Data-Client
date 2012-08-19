using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using CCTVClient.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CCTVClient.DataManagers
{
    
    public class MCUDataManager
    {
        public Dictionary<String, MCUDataAsset> DataItems;
        private bool autoUpdate=false;
        public bool hasSeenData = false;
        private bool directoryReady = false;
        public String xmlLocation
        {
            get;
            set;
        }

        private bool FileLocationValid=false;

        public MCUDataManager(String xmlFileLocation,bool autoUpdateIn=true)
        {
            DataItems = new Dictionary<string, MCUDataAsset>();
            autoUpdate=autoUpdateIn;
            xmlLocation = xmlFileLocation;
            importDefinitions();
        }

        public void AddMCUData(MCUDataAsset input){
            if (DataItems.ContainsKey(input.rawDataName))
            {
                DataItems[input.rawDataName] = input;
            }
            else
            {
                //.25 Regex is needed for misreads caused by incomplete transmissions, and for raw names that goof with parsing.
                if (Regex.IsMatch(input.rawDataName, @"^[A-Za-z0-9]*$"))
                {
                    DataItems.Add(input.rawDataName, input);
                    if (autoUpdate) { writeDataToXML(); }
                }
            }
        }

        public void AddMCUData(MCUDataAsset[] input){
            foreach(MCUDataAsset token in input){
                DataItems.Add(token.rawDataName, token);
            }
            if(autoUpdate){writeDataToXML();}
        }

        public void ReadMCUOutput(Dictionary<String, int> data)
        {
            if (directoryReady)
            {
                foreach (KeyValuePair<String, int> token in data)
                {
                    if (DataItems.ContainsKey(token.Key.Substring(1)))
                    {
                        DataItems[token.Key.Substring(1)].value = (Int32)token.Value;
                    }
                    else
                    {
                        AddMCUData(MCUAssetFactory.getInstance(token));
                    }
                }
                hasSeenData = true;
            }
            
        }

        public void ModifyMCUAttribute(String dataKey, UInt16 attribute, String inputData)
        {
            switch (attribute)
            {
                case MCUDataATTR.DATA_REFINED_NAME:
                    DataItems[dataKey].refinedDataName = inputData;
                    break;
                case MCUDataATTR.DATA_DETAILS:
                    DataItems[dataKey].dataDescription = inputData;
                    break;
                case MCUDataATTR.DATA_UNITS:
                    ((AnalogDataItem)DataItems[dataKey]).Units = inputData;
                    break;
                default:
                    break;
            }

            if (autoUpdate) {writeDataToXML(); }
        }

        public void ModifyMCUAttribute(String dataKey, UInt16 attribute, Int32 inputData)
        {
            switch (attribute)
            {
                case MCUDataATTR.DATA_MIN:
                     ((AnalogDataItem)DataItems[dataKey]).MinValue = inputData;
                    break;
                case MCUDataATTR.DATA_MAX:
                     ((AnalogDataItem)DataItems[dataKey]).MaxValue = inputData;
                    break;
                case MCUDataATTR.DATA_ACTUALMIN:
                     ((AnalogDataItem)DataItems[dataKey]).ActualMin = inputData;
                    break;
                case MCUDataATTR.DATA_ACTUALMAX:
                    ((AnalogDataItem)DataItems[dataKey]).ActualMax = inputData;
                    break;
                case MCUDataATTR.DATA_VALUE:
                    DataItems[dataKey].value = inputData;
                    break;
                default:
                    break;
            }

            if (autoUpdate) {writeDataToXML(); }
        }

        public void ModifyMCUValue(String dataKey,Int32 inputData)
        {
            DataItems[dataKey].value = inputData;
        }

        public String packageIntoXML()
        {
            String xmlString = String.Empty;
            xmlString+="<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
            xmlString+="<DATA lastSave='"+DateTime.Now.ToString()+"'>\r\n";
            foreach (KeyValuePair<String, MCUDataAsset> token in DataItems)
            {
                xmlString += token.Value.FormatDataForSave();
            }
            xmlString += "</DATA>\n";
            return xmlString;
        }

        public void writeDataToXML()
        {
            if (directoryReady)
            {
                System.IO.File.WriteAllText(@xmlLocation, packageIntoXML());
                Console.WriteLine("(writeDataToXML@MCUDataManager):Saved definitions");
            }
        }

        public void importDefinitions()
        {
            XmlTextReader reader = new XmlTextReader(xmlLocation);
            MCUDataAsset temp=new AnalogDataItem("temp");
            String currentType = "";
            String currentName = "";
            bool onInstance = false;
            int ct = 0;
            while (reader.Read())
            {
                ct++;
                if (currentType != "" && currentName != "")
                {
                    temp = MCUAssetFactory.getInstance(currentType, currentName);
                    currentName = "";
                    currentType = "";
                }
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "DATAINSTANCE":
                                currentType = reader.GetAttribute(0);
                                break;
                            case "RAWNAME":
                                currentName = reader.ReadInnerXml();                            
                                break;
                            case "REFINEDNAME":
                                temp.refinedDataName = reader.ReadInnerXml();
                                break;
                            case "DESCRIPTION":
                                temp.dataDescription = reader.ReadInnerXml();
                                break;
                            case "MINVALUE":
                                ((AnalogDataItem)temp).MinValue = Int32.Parse(reader.ReadInnerXml());
                                break;
                            case "MAXVALUE":
                                ((AnalogDataItem)temp).MaxValue = Int32.Parse(reader.ReadInnerXml());
                                break;
                            case "UNITS":
                                ((AnalogDataItem)temp).Units = reader.ReadInnerXml();
                                break;
                            case "MINACTUALVALUE":
                                ((AnalogDataItem)temp).ActualMin = Int32.Parse(reader.ReadInnerXml());
                                break;
                            case "MAXACTUALVALUE":
                                ((AnalogDataItem)temp).ActualMax = Int32.Parse(reader.ReadInnerXml());
                                break;
                            default:
                                break;
         

                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (reader.Name.Equals("DATAINSTANCE"))
                        {
                            AddMCUData(temp);
                        }
                        break;
                }
            }
            Console.WriteLine("(importDefinitions@MCUDataManager):Imported metaData");
            directoryReady = true;
            reader.Close();
        }
    }
}
