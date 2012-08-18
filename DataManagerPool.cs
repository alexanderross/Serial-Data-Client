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
        public SerialController DataLink;
        private bool dataReady = false;

        public DataManagerPool(String cfgLocale)
        {
            Config = new CFGDataManager(Directory.GetCurrentDirectory()+"//"+cfgLocale);
            DataLink= new SerialController(Config.GetConfigElement("serial_port_default"), int.Parse(Config.GetConfigElement("serial_rate_default")));
            MCU = new MCUDataManager(ReturnConfirmedLocale(Config.GetConfigElement("definition_location"), Config.GetConfigElement("definition_location")));
            Console.WriteLine("(INIT@DATAPOOL):Started Okay!");
            ApplyDetailedConfigItems();
        }

        public void ApplyDetailedConfigItems(){
            Console.WriteLine("(ApplyDetailedConfigItems@DATAPOOL):Applying advanced options.");
            foreach (KeyValuePair<string, string> cfgitem in Config.Data)
            {
                switch (cfgitem.Key)
                {
                        //Serial V0.4
                    case "serial_read_buffer_size":
                        DataLink.setInputBufferSize(int.Parse(cfgitem.Value));
                        break;
                    case "serial_data_bits":
                        DataLink.SetPortDataBits(int.Parse(cfgitem.Value));
                        break;
 	                case "serial_handshake":
                        DataLink.SetPortHandshake(cfgitem.Value.Trim());
                        break;
 	                case "serial_parity":
                        DataLink.SetPortParity(cfgitem.Value.Trim());
                        break;
 	                case "serial_stop_bits":
                        DataLink.SetPortStopBits(cfgitem.Value.Trim());
                        break;
 	                case "serial_broadcast_start":
                        DataLink.startCode = cfgitem.Value.Trim();
                        break;
                    case "serial_broadcast_stop":
                        DataLink.stopCode = cfgitem.Value.Trim();
                        break;
                }
            }
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

        public void Refresh(){
            if (DataLink.ActiveSerialPort.IsOpen)
            {
                MCU.ReadMCUOutput(DataLink.GetCurrentData());
            }
        }
    }
}
