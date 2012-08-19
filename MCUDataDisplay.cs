using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CCTVClient.Data
{
    public abstract class MCUDataDisplay:Panel
    {
        protected MCUDataAsset containedData;
        protected Label NameLabel;


        public MCUDataDisplay()
            : base()
        {
            NameLabel = new Label();

            this.BackColor = System.Drawing.Color.Transparent;      
            this.Size = new System.Drawing.Size(450, 25);
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.MinimumSize = new System.Drawing.Size(210, 25);
            this.NameLabel.Size = new System.Drawing.Size(210, 25);


            
            this.Controls.Add(NameLabel);
        }
        public abstract void InitializeElement();
        public abstract String formatDataForHTML();


        public abstract void RefreshData();
    }
}
