using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CCTVClient.Data
{
    class CompoundDataDisplay:MCUDataDisplay
    {
        public MCUDataAsset YAxisData;
        private Panel AxisDisplay;
        private Panel XLine,YLine;
        private Label XData,YData;
        private const int GridSize=150;

        public CompoundDataDisplay(MCUDataAsset XAxisData, MCUDataAsset YAxisDataIn)
            : base()
        {
            containedData=XAxisData;
            YAxisData=YAxisDataIn;
            NameLabel.Text = "AXIS:" + XAxisData.rawDataName + "," + YAxisData.rawDataName;
            AxisDisplay = new Panel();
            XLine = new Panel();
            YLine = new Panel();
            XData = new Label();
            YData = new Label();
            this.Controls.Add(XData);
            this.Controls.Add(YData);
            AxisDisplay.Controls.Add(XLine);
            AxisDisplay.Controls.Add(YLine);
            this.Controls.Add(AxisDisplay);
            XLine.Size= new System.Drawing.Size(2,GridSize);
            YLine.Size= new System.Drawing.Size(GridSize,2);
            XLine.BackColor=System.Drawing.Color.White;
            YLine.BackColor=System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(GridSize+52, GridSize+52);
            AxisDisplay.Size = new System.Drawing.Size(GridSize+2,GridSize+2);
            AxisDisplay.AutoSize = false;
            this.AxisDisplay.Location = new System.Drawing.Point(0, 50);

            XData.ForeColor = System.Drawing.Color.White;
            XData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            XData.Text = "VAL";
            XData.Size = new System.Drawing.Size(100, 25);
            XData.Location = new System.Drawing.Point(0, 25);
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            YData.ForeColor = System.Drawing.Color.White;
            YData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            YData.Text = "VAL";
            YData.Size = new System.Drawing.Size(100, 25);
            YData.Location = new System.Drawing.Point(100, 25);

        }

        override public void InitializeElement()
        {

        }

        override public String formatDataForHTML()
        {
            return String.Empty;
        }

        public override void RefreshData()
        {
            this.Invoke((MethodInvoker)delegate
            {
               XData.Text=containedData.rawDataName+":"+containedData.GetValueFormatted();
               YData.Text=YAxisData.rawDataName+":"+YAxisData.GetValueFormatted();
               XLine.Location=new System.Drawing.Point((int)(((AnalogDataItem)containedData).GetPct()*GridSize),0);
               YLine.Location=new System.Drawing.Point(0,(int)(((AnalogDataItem)YAxisData).GetPct()*GridSize));
            });
        }
    }
}
