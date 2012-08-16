namespace CCTVClient
{
    partial class Setup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VersionLabel = new System.Windows.Forms.Label();
            this.sensorReadingsHeader = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.PLACEHOLDER = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(13, 13);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(32, 17);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "VER";
            // 
            // sensorReadingsHeader
            // 
            this.sensorReadingsHeader.AutoSize = true;
            this.sensorReadingsHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensorReadingsHeader.Location = new System.Drawing.Point(11, 30);
            this.sensorReadingsHeader.Name = "sensorReadingsHeader";
            this.sensorReadingsHeader.Size = new System.Drawing.Size(466, 30);
            this.sensorReadingsHeader.TabIndex = 2;
            this.sensorReadingsHeader.Text = "Sensor Readings                                                  ";
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(16, 64);
            this.nameLabel.MinimumSize = new System.Drawing.Size(210, 25);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(210, 25);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "label1dddddddfffgfgfdgdddd";
            // 
            // PLACEHOLDER
            // 
            this.PLACEHOLDER.BackColor = System.Drawing.Color.Transparent;
            this.PLACEHOLDER.Location = new System.Drawing.Point(16, 63);
            this.PLACEHOLDER.Name = "PLACEHOLDER";
            this.PLACEHOLDER.Size = new System.Drawing.Size(450, 265);
            this.PLACEHOLDER.TabIndex = 4;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 328);
            this.Controls.Add(this.PLACEHOLDER);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.sensorReadingsHeader);
            this.Controls.Add(this.VersionLabel);
            this.Name = "Setup";
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label sensorReadingsHeader;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Panel PLACEHOLDER;
    }
}