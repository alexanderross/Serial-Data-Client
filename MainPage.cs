using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Text.RegularExpressions;
using CCTVClient;
using CCTVClient.Web;


namespace CCTVClient
{
    public partial class MainPage : Form
    {
        public DataManagerPool dataController;
        public SerialController mySerialConn;
        public ContextPanel activeContext;
        public CCTVWebServer HTTPServerInstance;
        private System.Timers.Timer myTimer;
        String[] lightLevels;

        public MainPage()
        {
            //lightLevels = new String[10] { "Pitch Black", "Dark", "Shady", "OK", "Light", "Very Light", "Excessively Bright", "Non-Ambient Light", "Definitely Direct Synthetic Light", "Very Close Synthetic Light" };
            myTimer = new System.Timers.Timer(100);
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            myTimer.AutoReset = true;
            InitializeComponent();
            dataController = new DataManagerPool("CCTV.cfg");

            HTTPServerInstance = new CCTVWebServer(dataController,int.Parse(dataController.Config.GetConfigElement("http_port_default")),"www");

            mySerialConn = new SerialController(dataController.Config.GetConfigElement("serial_port_default"), int.Parse(dataController.Config.GetConfigElement("serial_rate_default")));

            VersionLabel.Text = "Version: "+dataController.Config.GetConfigElement("version_number") + dataController.Config.GetConfigElement("build_date");
            mySerialConn.StartRx();
            this.activeContext = new MainPanel(this);
            this.contextContainer.Controls.Add(this.activeContext);
            ((MainPanel)this.activeContext).InitSensorAndDetails();
            myTimer.Start();
        }

        public void setContext(String contextLabel)
        {
        }

        public void setDisplay(String newText)
        {
            
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            int value = 0;
            this.activeContext.updateData();
            if (this.mySerialConn.ActiveSerialPort.IsOpen)
            {
                this.SerialActiveLabel.Text = "CONNECTED";
                this.Invoke((MethodInvoker)delegate { this.SerialActiveLabel.ForeColor = Color.Green; });
                dataController.MCU.ReadMCUOutput(mySerialConn.GetCurrentData());
            }
            else
            {
                this.SerialActiveLabel.Text = "DISCONNECTED";
                this.Invoke((MethodInvoker)delegate { this.SerialActiveLabel.ForeColor = Color.Red; });
            }

            if (this.HTTPServerInstance.IsAlive)
            {
                this.HHTPActiveLabel.Text = "CONNECTED";
                this.Invoke((MethodInvoker)delegate { this.HHTPActiveLabel.ForeColor = Color.Green; });
                this.HostStatusLabel.Text = "Host: " + HTTPServerInstance.getPort();
            }
            else
            {
                this.HHTPActiveLabel.Text = "DISCONNECTED";
                this.Invoke((MethodInvoker)delegate { this.HHTPActiveLabel.ForeColor = Color.Red; });
            }

            this.statusActivePortName.Text = "Port:"+this.mySerialConn.ActiveSerialPort.PortName;
            this.statusActivePortBaud.Text = "Rate:" + this.mySerialConn.ActiveSerialPort.BaudRate.ToString();   
        }



        private void HHTPActiveLabel_Click(object sender, EventArgs e)
        {
            if (this.HTTPServerInstance.IsAlive)
            {
                HTTPServerInstance.Stop();
            }
            else
            {
                HTTPServerInstance.Start();
            }
        }

        private void SerialActiveLabel_Click(object sender, EventArgs e)
        {
            if (!this.mySerialConn.ActiveSerialPort.IsOpen)
            {
                mySerialConn.StartRx();
            }
            else
            {
                mySerialConn.Disconnect();
            }
        }

        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.mySerialConn.ActiveSerialPort.IsOpen)
            {
                this.Invoke((MethodInvoker)delegate {mySerialConn.Disconnect(); });
            }
            if (this.HTTPServerInstance.IsAlive)
            {
                HTTPServerInstance.Stop();
            }

        }
    }
}
