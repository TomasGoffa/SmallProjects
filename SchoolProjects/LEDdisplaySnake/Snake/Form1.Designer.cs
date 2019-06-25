namespace Snake
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.LblTrajectory = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnSegmentsDec = new System.Windows.Forms.Button();
            this.BtnSegmentsInc = new System.Windows.Forms.Button();
            this.BtnChangeTrajectory = new System.Windows.Forms.Button();
            this.BtnSpeedDec = new System.Windows.Forms.Button();
            this.BtnSpeedInc = new System.Windows.Forms.Button();
            this.LblSpeed = new System.Windows.Forms.Label();
            this.LblSegments = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblTrajectory
            // 
            this.LblTrajectory.AutoSize = true;
            this.LblTrajectory.Location = new System.Drawing.Point(169, 44);
            this.LblTrajectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblTrajectory.Name = "LblTrajectory";
            this.LblTrajectory.Size = new System.Drawing.Size(46, 17);
            this.LblTrajectory.TabIndex = 0;
            this.LblTrajectory.Text = "label1";
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 1;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // BtnSegmentsDec
            // 
            this.BtnSegmentsDec.Location = new System.Drawing.Point(16, 284);
            this.BtnSegmentsDec.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSegmentsDec.Name = "BtnSegmentsDec";
            this.BtnSegmentsDec.Size = new System.Drawing.Size(100, 28);
            this.BtnSegmentsDec.TabIndex = 1;
            this.BtnSegmentsDec.Text = "- Segments";
            this.BtnSegmentsDec.UseVisualStyleBackColor = true;
            this.BtnSegmentsDec.Click += new System.EventHandler(this.BtnSegmentsDec_Click);
            // 
            // BtnSegmentsInc
            // 
            this.BtnSegmentsInc.Location = new System.Drawing.Point(16, 249);
            this.BtnSegmentsInc.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSegmentsInc.Name = "BtnSegmentsInc";
            this.BtnSegmentsInc.Size = new System.Drawing.Size(100, 28);
            this.BtnSegmentsInc.TabIndex = 2;
            this.BtnSegmentsInc.Text = "+ Segments";
            this.BtnSegmentsInc.UseVisualStyleBackColor = true;
            this.BtnSegmentsInc.Click += new System.EventHandler(this.BtnSegmentsInc_Click);
            // 
            // BtnChangeTrajectory
            // 
            this.BtnChangeTrajectory.Location = new System.Drawing.Point(97, 213);
            this.BtnChangeTrajectory.Margin = new System.Windows.Forms.Padding(4);
            this.BtnChangeTrajectory.Name = "BtnChangeTrajectory";
            this.BtnChangeTrajectory.Size = new System.Drawing.Size(193, 28);
            this.BtnChangeTrajectory.TabIndex = 3;
            this.BtnChangeTrajectory.Text = "Change trajectory";
            this.BtnChangeTrajectory.UseVisualStyleBackColor = true;
            this.BtnChangeTrajectory.Click += new System.EventHandler(this.BtnChangeTrajectory_Click);
            // 
            // BtnSpeedDec
            // 
            this.BtnSpeedDec.Location = new System.Drawing.Point(273, 284);
            this.BtnSpeedDec.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSpeedDec.Name = "BtnSpeedDec";
            this.BtnSpeedDec.Size = new System.Drawing.Size(100, 28);
            this.BtnSpeedDec.TabIndex = 5;
            this.BtnSpeedDec.Text = "- Speed";
            this.BtnSpeedDec.UseVisualStyleBackColor = true;
            this.BtnSpeedDec.Click += new System.EventHandler(this.BtnSpeedDec_Click);
            // 
            // BtnSpeedInc
            // 
            this.BtnSpeedInc.Location = new System.Drawing.Point(273, 249);
            this.BtnSpeedInc.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSpeedInc.Name = "BtnSpeedInc";
            this.BtnSpeedInc.Size = new System.Drawing.Size(100, 28);
            this.BtnSpeedInc.TabIndex = 6;
            this.BtnSpeedInc.Text = "+ Speed";
            this.BtnSpeedInc.UseVisualStyleBackColor = true;
            this.BtnSpeedInc.Click += new System.EventHandler(this.BtnSpeedInc_Click);
            // 
            // LblSpeed
            // 
            this.LblSpeed.AutoSize = true;
            this.LblSpeed.Location = new System.Drawing.Point(169, 140);
            this.LblSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSpeed.Name = "LblSpeed";
            this.LblSpeed.Size = new System.Drawing.Size(46, 17);
            this.LblSpeed.TabIndex = 7;
            this.LblSpeed.Text = "label2";
            // 
            // LblSegments
            // 
            this.LblSegments.AutoSize = true;
            this.LblSegments.Location = new System.Drawing.Point(169, 91);
            this.LblSegments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSegments.Name = "LblSegments";
            this.LblSegments.Size = new System.Drawing.Size(46, 17);
            this.LblSegments.TabIndex = 8;
            this.LblSegments.Text = "label2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 327);
            this.Controls.Add(this.LblSegments);
            this.Controls.Add(this.LblSpeed);
            this.Controls.Add(this.BtnSpeedInc);
            this.Controls.Add(this.BtnSpeedDec);
            this.Controls.Add(this.BtnChangeTrajectory);
            this.Controls.Add(this.BtnSegmentsInc);
            this.Controls.Add(this.BtnSegmentsDec);
            this.Controls.Add(this.LblTrajectory);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblTrajectory;
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Button BtnSegmentsDec;
        private System.Windows.Forms.Button BtnSegmentsInc;
        private System.Windows.Forms.Button BtnChangeTrajectory;
        private System.Windows.Forms.Button BtnSpeedDec;
        private System.Windows.Forms.Button BtnSpeedInc;
        private System.Windows.Forms.Label LblSpeed;
        private System.Windows.Forms.Label LblSegments;
    }
}

