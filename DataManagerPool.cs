using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CCTVClient.DataManagers;

namespace CCTVClient
{
    public class DataManagerPool
    {
        public CFGDataManager Config;
        public MCUDataManager MCU;
        private bool dataReady = false;

        public DataManagerPool(String cfgLocale)
        {
            Config = new CFGDataManager(Directory.GetCurrentDirectory()+"//"+cfgLocale);
            MCU = new MCUDataManager(ReturnConfirmedLocale(Config.GetConfigElement("definition_location"), Config.GetConfigElement("definition_location")));
            Console.WriteLine("(INIT@DATAPOOL):Started Okay!");
        }

        public String ReturnConfirmedLocale(String name,String type)
        {
            if (!System.IO.File.Exists(name))
            {
                DialogResult dlgRes = MessageBox.Show(
                 "The " + type + " file wasn't found, one will be created now.",
                 "Create New " + type + " File",
                MessageBoxButtons.OK,
                MessageBoxIcon.Question);

                System.IO.File.Create(type);
                return type;
                Config.Data["definition_location"] = Directory.GetCurrentDirectory()+"//"+type;
            }
            else{ return name; }
        }
    }
}
