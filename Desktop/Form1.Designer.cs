namespace Desktop
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Komponen UI
        private Button btnPilihCSV;
        private Button btnPilihLogo;
        private Button btnGeneratePDF;
        private Label lblCSV;
        private Label lblLogo;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnPilihCSV = new System.Windows.Forms.Button();
            this.btnPilihLogo = new System.Windows.Forms.Button();
            this.btnGeneratePDF = new System.Windows.Forms.Button();
            this.lblCSV = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPilihCSV
            // 
            this.btnPilihCSV.Location = new System.Drawing.Point(40, 40);
            this.btnPilihCSV.Name = "btnPilihCSV";
            this.btnPilihCSV.Size = new System.Drawing.Size(150, 40);
            this.btnPilihCSV.TabIndex = 0;
            this.btnPilihCSV.Text = "Pilih File CSV";
            this.btnPilihCSV.UseVisualStyleBackColor = true;
            this.btnPilihCSV.Click += new System.EventHandler(this.btnPilihCSV_Click);
            // 
            // lblCSV
            // 
            this.lblCSV.AutoSize = true;
            this.lblCSV.Location = new System.Drawing.Point(210, 50);
            this.lblCSV.Name = "lblCSV";
            this.lblCSV.Size = new System.Drawing.Size(120, 25);
            this.lblCSV.TabIndex = 1;
            this.lblCSV.Text = "Belum dipilih";
            // 
            // btnPilihLogo
            // 
            this.btnPilihLogo.Location = new System.Drawing.Point(40, 100);
            this.btnPilihLogo.Name = "btnPilihLogo";
            this.btnPilihLogo.Size = new System.Drawing.Size(150, 40);
            this.btnPilihLogo.TabIndex = 2;
            this.btnPilihLogo.Text = "Pilih Logo";
            this.btnPilihLogo.UseVisualStyleBackColor = true;
            this.btnPilihLogo.Click += new System.EventHandler(this.btnPilihLogo_Click);
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Location = new System.Drawing.Point(210, 110);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(120, 25);
            this.lblLogo.TabIndex = 3;
            this.lblLogo.Text = "Belum dipilih";
            // 
            // btnGeneratePDF
            // 
            this.btnGeneratePDF.Location = new System.Drawing.Point(40, 170);
            this.btnGeneratePDF.Name = "btnGeneratePDF";
            this.btnGeneratePDF.Size = new System.Drawing.Size(320, 45);
            this.btnGeneratePDF.TabIndex = 4;
            this.btnGeneratePDF.Text = "Generate PDF";
            this.btnGeneratePDF.UseVisualStyleBackColor = true;
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(40, 230);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 25);
            this.lblStatus.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 300);
            this.Controls.Add(this.btnPilihCSV);
            this.Controls.Add(this.lblCSV);
            this.Controls.Add(this.btnPilihLogo);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.btnGeneratePDF);
            this.Controls.Add(this.lblStatus);
            this.Name = "Form1";
            this.Text = "Konversi CSV ke PDF";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}