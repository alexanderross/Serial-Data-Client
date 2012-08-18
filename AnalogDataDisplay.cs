using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CCTVClient.Data
{
    public class AnalogDataDisplay : MCUDataDisplay
    {
        private Label valueDisplay;
        private Panel slideDataDisplay;
        public AnalogDataDisplay(MCUDataAsset input):base(input)
        {
            containedData = input;
            InitializeElement();
            valueDisplay = new Label();
            slideDataDisplay = new Panel();
            valueDisplay.AutoSize = false;
            valueDisplay.BackColor = System.Drawing.Color.FromArgb(0,255,255,255);
            this.Controls.Add(slideDataDisplay);
            this.slideDataDisplay.Controls.Add(valueDisplay);
            slideDataDisplay.Location = new System.Drawing.Point(210, 0);
            slideDataDisplay.BackColor = System.Drawing.Color.Green;
           // valueDisplay.Location = new System.Drawing.Point(210,0);
            valueDisplay.Size = new System.Drawing.Size(100, 25);
            valueDisplay.ForeColor = System.Drawing.Color.White;
            valueDisplay.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueDisplay.Text = "VAL";
            valueDisplay.Size = new System.Drawing.Size(200, 25);
            valueDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.Invoke((MethodInvoker)delegate {
            String newText = ((AnalogDataItem)containedData).GetValueFormatted();
            valueDisplay.Text = newText; 
            double newWidth=(((AnalogDataItem)containedData).MaxValue-((AnalogDataItem)containedData).MinValue);
                newWidth=(((AnalogDataItem)containedData).GetRealValue()-((AnalogDataItem)containedData).MinValue)/newWidth;
                if (newWidth >= 0 && newWidth <= 1)
                {
                    slideDataDisplay.BackColor = System.Drawing.Color.FromArgb((int)(255 - (newWidth * 255)), (int)(0 + (newWidth * 255)), 0);
                    slideDataDisplay.Width = (int)(170 * newWidth) + 30;
                }
             });
        }
    }
}
