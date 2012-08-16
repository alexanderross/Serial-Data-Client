using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CCTVClient.DataManagers
{
    public class CFGDataManager
    {
        public Dictionary<String, String> Data;
        private Dictionary<String,String> CrucialData;
        private string writeLocale;

        public CFGDataManager(String configLocation){
            Data = new Dictionary<string,string>();
            writeLocale = configLocation;
            initiateCrucialData();
            readConfigFile();
        }

        private void initiateCrucialData(){
            CrucialData = new Dictionary<string,string>();
            CrucialData.Add("definition_location","definitions.xml");
            CrucialData.Add("serial_port_default","COM3");
            CrucialData.Add("serial_rate_default","9600");
            CrucialData.Add("serial_delay_period","100");
            CrucialData.Add("http_port_default","8080");
        }

        private void readConfigFile()
        {
            String[] temporaryItem;
            bool continueTask=true;
            if (!System.IO.File.Exists(writeLocale))
            {
                DialogResult dlgRes = MessageBox.Show(
                 "The Config file wasn't found at " + writeLocale + ", would you like to create one at your current location?", 
                 "Create New CFG File", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                {
                    System.IO.File.Create(writeLocale);
                }
                else
                {
                    continueTask = false;
                }
            }
            if (continueTask)
            {
                String[] configItems = System.IO.File.ReadAllLines(writeLocale);
                foreach (String configItem in configItems)
                {
                    if (!configItem.Contains("//") && configItem.Trim()!="")
                    {
                        temporaryItem = configItem.Split('=');
                        try
                        {
                            Data.Add(temporaryItem[0], temporaryItem[1]);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }

        public void writeConfigFile()
        {
            if (File.Exists(writeLocale))
            {
                using (StreamWriter sw = new StreamWriter(writeLocale))
                {
                    //System.IO.File.WriteAllText(@writeLocale, string.Empty);
                    foreach (KeyValuePair<String,String> item in Data)
                    {
                        sw.WriteLine(item.Key+"="+item.Value);
                    }
                }
            }
        }

        public String GetConfigElement(string key)
        {
            if (Data.ContainsKey(key))
            {
                return Data[key];
            }
            else
            {
                initiateCrucialData();
                if(CrucialData.ContainsKey(key)){
                    addConfigParam(key,CrucialData[key]);
                    Console.WriteLine("(GetConfigElement@CFGDataManager):Crucial param not in CFG, provided default..");
                    return CrucialData[key];
                }else{
                    DialogResult dlgRes = MessageBox.Show("A crucial config parameter, '"+key+"', was missing. Please locate your CCTV.cfg file and check that this parameter is properly defined. Thanks.");
                    Application.Exit();
                    return "";
                }
            }
        }

        public void addConfigParam(string key, string value)
        {
            Data.Add(key, value);
            writeConfigFile();
        }
    }
}
