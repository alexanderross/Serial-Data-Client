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
        private Dictionary<string,MCUDataDisplay> dataLabels;
        private List<int> dataLabelHeights;
        private int paramNumber = 0;
        private bool populated = false;

        public MainPanel(MainPage inputParent){
            parent = inputParent;
            type = "MAIN";
        }

        public void InitSensorAndDetails(){
            dataLabels = new Dictionary<string, MCUDataDisplay>();
            this.SensorReadingsContainer = new Panel();
            this.sensorReadingsHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // SensorReading Container
            // 
            this.SensorReadingsContainer.Location = new System.Drawing.Point(12, 30);
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
            this.sensorReadingsHeader.Location = new System.Drawing.Point(12, 0);
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

            //populateParams();
        }

        public void populateParams()
        {
                if (parent.dataController.MCU.hasSeenData)
                {
                    Console.WriteLine("(populateParams@DataPanel):Did it");
                    foreach (KeyValuePair<String, MCUDataAsset> token in parent.dataController.MCU.DataItems)
                    {
                        AddDataLabel(token.Value.rawDataName,token.Value);

                    }
                    populated = true;
                }
        }

        private void AddDataLabel(String name, MCUDataAsset data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                dataLabels.Add(name, DataDisplayFactory.GetInstance(data));
                this.SensorReadingsContainer.Controls.Add(dataLabels[name]);
                dataLabels[name].Location = new System.Drawing.Point(12, dataLabelHeights[paramNumber]);
                paramNumber++;
            });
        }

        private void AddComplexDisplay()
        {
            this.Invoke((MethodInvoker)delegate
            {
                dataLabels.Add("AXISDISP", new Data.CompoundDataDisplay(parent.dataController.MCU.DataItems["XAXIS"],parent.dataController.MCU.DataItems["YAXIS"]));
                this.SensorReadingsContainer.Controls.Add(dataLabels["AXISDISP"]);
                dataLabels["AXISDISP"].Location = new System.Drawing.Point(12, dataLabelHeights[paramNumber]);
                paramNumber++;
                populated = true;
            });
        }

        private void RefreshLabels()
        {
            this.SensorReadingsContainer.Controls.Clear();
            dataLabels.Clear();
            paramNumber = 0;
            populated = false;
        }

        public override void updateData()
        {
            if (!populated)
            {
                AddComplexDisplay();
                //populateParams();
            }else{
                this.Invoke((MethodInvoker)delegate
                {
                    dataLabels["AXISDISP"].RefreshData();
                });
            }/*
            foreach (MCUDataAsset token in parent.dataController.MCU.DataItems.Values)
            {
                if (dataLabels.ContainsKey(token.rawDataName))
                {
                    dataLabels[token.rawDataName].RefreshData();
                }
                else
                {
                    AddDataLabel(token.rawDataName, token);
                }
            }*/



        }
    }
}
