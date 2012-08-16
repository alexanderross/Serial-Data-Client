using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CCTVClient.Data
{
    public class DigitalDataDisplay : MCUDataDisplay
    {
        private Label valueDisplay;
        public DigitalDataDisplay(MCUDataAsset item)
            : base(item)
        {
            valueDisplay = new Label();
            valueDisplay.Location = new System.Drawing.Point(210,0);
            this.Controls.Add(valueDisplay);
            valueDisplay.Size = new System.Drawing.Size(100, 25);
            valueDisplay.Text = "VAL";
            containedData = item;
            InitializeElement();
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
            this.Invoke((MethodInvoker)delegate { valueDisplay.Text = ((DigitalDataItem)containedData).GetValueFormatted(); });
        }
    }
}
