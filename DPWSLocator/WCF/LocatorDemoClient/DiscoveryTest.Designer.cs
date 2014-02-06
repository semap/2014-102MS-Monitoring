namespace LocatorDemoClient
{
    partial class DiscoveryTest
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
            this.buttonProbe = new System.Windows.Forms.Button();
            this.labelProbeResult = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAnnouncement = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonProbe
            // 
            this.buttonProbe.Location = new System.Drawing.Point(60, 52);
            this.buttonProbe.Name = "buttonProbe";
            this.buttonProbe.Size = new System.Drawing.Size(75, 23);
            this.buttonProbe.TabIndex = 0;
            this.buttonProbe.Text = "Probe";
            this.buttonProbe.UseVisualStyleBackColor = true;
            this.buttonProbe.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelProbeResult
            // 
            this.labelProbeResult.AutoSize = true;
            this.labelProbeResult.Location = new System.Drawing.Point(199, 62);
            this.labelProbeResult.Name = "labelProbeResult";
            this.labelProbeResult.Size = new System.Drawing.Size(0, 13);
            this.labelProbeResult.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Announcements";
            // 
            // labelAnnouncement
            // 
            this.labelAnnouncement.AutoSize = true;
            this.labelAnnouncement.Location = new System.Drawing.Point(199, 109);
            this.labelAnnouncement.Name = "labelAnnouncement";
            this.labelAnnouncement.Size = new System.Drawing.Size(0, 13);
            this.labelAnnouncement.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "-->";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "-->";
            // 
            // DiscoveryTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 192);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelAnnouncement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelProbeResult);
            this.Controls.Add(this.buttonProbe);
            this.Name = "DiscoveryTest";
            this.Text = "Discovery Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonProbe;
        private System.Windows.Forms.Label labelProbeResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelAnnouncement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

