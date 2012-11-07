using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace CCTVClient
{
    public class SerialController
    {
        private int READ_SPEED = 10;
        static bool _continue;
        public SerialPort ActiveSerialPort;
        static Thread _readThread;
        private int dataCount = 0;
        private int badDataCount = 0;
        private String input="";
        private String[] inputArray;
        public string startCode;
        public string stopCode;

        public SerialController(String defPort,int defBaud)
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            // Create a new ActiveSerialPort object with default settings.
            ActiveSerialPort =new SerialPort(defPort, defBaud, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            dataCount++;    
            startCode = "GO!";
            stopCode = "STOP!";
        }

        public void Read()
        {
            while (_continue)
            {
                System.Threading.Thread.Sleep(READ_SPEED);
                input = ActiveSerialPort.ReadLine();
                //inputArray = Regex.Split(ActiveSerialPort.ReadExisting(), "\r\n");
                //input = inputArray[2];
                if (ActiveSerialPort.BytesToRead > 1000)
                {
                    Console.WriteLine("(Read@SerialController):Buffer's getting full, speed up your read interval to match the serial rate.");
                    ActiveSerialPort.DiscardInBuffer();
                }
                //Console.WriteLine(input);
                //ActiveSerialPort.DiscardInBuffer();
                dataCount++;
            }
        }

        public void Write()
        {
        }

        public void StartRx()
        {
            if (ActiveSerialPort.PortName != null)
            {
                try
                {
                    ActiveSerialPort.Open();
                    ActiveSerialPort.DiscardInBuffer();
                    _continue = true;
                    StartTx(startCode);
                    _readThread = new Thread(Read);
                    _readThread.Start();
                }
                catch (IOException exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.Message + " Please check the settings file and ensure that your settings are correct.");
                }
            }
        }

        public void StartTx(String message)
        {
            if (ActiveSerialPort.IsOpen)
            {
                ActiveSerialPort.Write(message);
            }
        }

        public void Disconnect()
        {
            StartTx(stopCode);
            _continue = false;
            _readThread.Join();
            ActiveSerialPort.Close();
        }

        public Dictionary<String,int> GetCurrentData()
        {
            Dictionary<String, int> returnValue = new Dictionary<string, int>();
                if (input.Contains(":"))
                {
                    try
                    {
                        input = input.Replace("<DATA>", "");
                        String[] currentData = input.Split(',');
                        foreach (String s in currentData)
                        {
                            String[] dataItem = s.Split(':');
                            returnValue.Add(dataItem[0], int.Parse(dataItem[1]));
                        }
                        int h = 0;
                    }
                    catch (Exception eX)
                    {
                        badDataCount++;
                        Console.Write("IMPROPER FORMAT DETECTED FROM MCU");
                    }
                }
            return returnValue;
          
        }

        public int getReadDelay()
        {
            return READ_SPEED;
        }

        public void setReadDelay(int delay)
        {
            READ_SPEED = delay;
        }


        public void setInputBufferSize(int size)
        {
            ActiveSerialPort.ReadBufferSize = size;
        }

        public String[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        public void setPort(String port)
        {

            ActiveSerialPort.PortName = port;
        }

        public void SetPortBaudRate(int inputBaudRate)
        {
            ActiveSerialPort.BaudRate = inputBaudRate;
        }

        public String[] GetAvailablePortParities()
        {
            return Enum.GetNames(typeof(Parity));
        }

        public void SetPortParity(String parity)
        {  
            ActiveSerialPort.Parity=(Parity)Enum.Parse(typeof(Parity), parity);
        }

        public int GetCurrentPortDataBits()
        {
            return ActiveSerialPort.DataBits;
        }

        public void SetPortDataBits(int portDataBits)
        {
            ActiveSerialPort.DataBits = portDataBits;
        }

        public String[] GetPortStopBits()
        {
            return Enum.GetNames(typeof(StopBits));
        }

        public void SetPortStopBits(String portStopBits)
        {
            
            ActiveSerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), portStopBits);
        }

        public String[] GetPortHandshakes()
        {
            return Enum.GetNames(typeof(Handshake));
        }

        public void SetPortHandshake(String portHandshake)
        {
            
            ActiveSerialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), portHandshake);
        }
    }
}
