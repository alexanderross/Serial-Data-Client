using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCTVClient.Data;


namespace CCTVClient
{
   public class MainPanel:ContextPanel
    {
        private System.Windows.Forms.Label sensorReadingsHeader;
        private Panel SensorReadingsContainer; 
        private List<MCUDataDisplay> dataLabels;
        private List<int> dataLabelHeights;
        private int paramNumber = 0;

        public MainPanel(MainPage inputParent){
            parent = inputParent;
            type = "MAIN";
            if (parent.mySerialConn.ActiveSerialPort.IsOpen)
            {
                
            }
            else
            {

            }
        }

        public void InitSensorAndDetails(){
            dataLabels = new List<MCUDataDisplay>();
            this.SensorReadingsContainer = new Panel();
            this.sensorReadingsHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // SensorReading Container
            // 
            this.SensorReadingsContainer.Location = new System.Drawing.Point(12, 100);
            this.SensorReadingsContainer.Size = new System.Drawing.Size(466, 265);
            this.SensorReadingsContainer.AutoSize=false;
            this.SensorReadingsContainer.BackColor = System.Drawing.Color.Transparent;
            dataLabelHeights = new List<int>(); 
            for(int i=0;i<SensorReadingsContainer.Height;i+=25){
                dataLabelHeights.Add(i);
            }

            // 
            // sensorReadingsHeader
            // 
            this.sensorReadingsHeader.AutoSize = true;
            this.sensorReadingsHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensorReadingsHeader.Location = new System.Drawing.Point(12, 71);
            this.sensorReadingsHeader.Name = "sensorReadingsHeader";
            this.sensorReadingsHeader.Size = new System.Drawing.Size(466, 30);
            this.sensorReadingsHeader.TabIndex = 2;
            this.sensorReadingsHeader.Text = "Sensor Readings                                                  ";
            // 
            // Setup
            // 
            this.Controls.Add(this.sensorReadingsHeader);
            this.Controls.Add(this.SensorReadingsContainer);
            this.ClientSize = new System.Drawing.Size(484, 328);
            this.ResumeLayout(false);
            this.PerformLayout();
            System.Threading.Thread.Sleep(500);
            sensorReadingsHeader.Text = "";

            populateParams();
        }

        public void populateParams()
        {
            int ct = 0;
            foreach (KeyValuePair<String, MCUDataAsset> token in parent.dataController.MCU.DataItems)
            {
                dataLabels.Add(DataDisplayFactory.GetInstance(token.Value));
                this.SensorReadingsContainer.Controls.Add(dataLabels[ct]);
                dataLabels[ct].Location = new System.Drawing.Point(12, dataLabelHeights[ct]);
                ct++;
            }
        }

        public override void updateData()
        {
            foreach (MCUDataDisplay disp in dataLabels)
            {
                disp.RefreshData();
            }
        }
    }
}
