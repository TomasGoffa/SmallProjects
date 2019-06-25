namespace VoltageGenerator
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BtnRectangle = new System.Windows.Forms.Button();
            this.BtnSaw = new System.Windows.Forms.Button();
            this.BtnTriangle = new System.Windows.Forms.Button();
            this.BtnSinus = new System.Windows.Forms.Button();
            this.ampLabel = new System.Windows.Forms.Label();
            this.freqLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 10;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(16, 15);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1235, 480);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // BtnRectangle
            // 
            this.BtnRectangle.Location = new System.Drawing.Point(1259, 15);
            this.BtnRectangle.Margin = new System.Windows.Forms.Padding(4);
            this.BtnRectangle.Name = "BtnRectangle";
            this.BtnRectangle.Size = new System.Drawing.Size(100, 28);
            this.BtnRectangle.TabIndex = 1;
            this.BtnRectangle.Text = "Rectangle";
            this.BtnRectangle.UseVisualStyleBackColor = true;
            this.BtnRectangle.Click += new System.EventHandler(this.BtnRectangle_Click);
            // 
            // BtnSaw
            // 
            this.BtnSaw.Location = new System.Drawing.Point(1259, 50);
            this.BtnSaw.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSaw.Name = "BtnSaw";
            this.BtnSaw.Size = new System.Drawing.Size(100, 28);
            this.BtnSaw.TabIndex = 2;
            this.BtnSaw.Text = "Saw";
            this.BtnSaw.UseVisualStyleBackColor = true;
            this.BtnSaw.Click += new System.EventHandler(this.BtnSaw_Click);
            // 
            // BtnTriangle
            // 
            this.BtnTriangle.Location = new System.Drawing.Point(1259, 86);
            this.BtnTriangle.Margin = new System.Windows.Forms.Padding(4);
            this.BtnTriangle.Name = "BtnTriangle";
            this.BtnTriangle.Size = new System.Drawing.Size(100, 28);
            this.BtnTriangle.TabIndex = 3;
            this.BtnTriangle.Text = "Triangle";
            this.BtnTriangle.UseVisualStyleBackColor = true;
            this.BtnTriangle.Click += new System.EventHandler(this.BtnTriangle_Click);
            // 
            // BtnSinus
            // 
            this.BtnSinus.Location = new System.Drawing.Point(1259, 122);
            this.BtnSinus.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSinus.Name = "BtnSinus";
            this.BtnSinus.Size = new System.Drawing.Size(100, 28);
            this.BtnSinus.TabIndex = 4;
            this.BtnSinus.Text = "Sinus";
            this.BtnSinus.UseVisualStyleBackColor = true;
            this.BtnSinus.Click += new System.EventHandler(this.BtnSinus_Click);
            // 
            // ampLabel
            // 
            this.ampLabel.AutoSize = true;
            this.ampLabel.Location = new System.Drawing.Point(288, 505);
            this.ampLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ampLabel.Name = "ampLabel";
            this.ampLabel.Size = new System.Drawing.Size(46, 17);
            this.ampLabel.TabIndex = 5;
            this.ampLabel.Text = "label1";
            // 
            // freqLabel
            // 
            this.freqLabel.AutoSize = true;
            this.freqLabel.Location = new System.Drawing.Point(952, 505);
            this.freqLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.freqLabel.Name = "freqLabel";
            this.freqLabel.Size = new System.Drawing.Size(46, 17);
            this.freqLabel.TabIndex = 7;
            this.freqLabel.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1053, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1057, 204);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "label2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1372, 596);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.freqLabel);
            this.Controls.Add(this.ampLabel);
            this.Controls.Add(this.BtnSinus);
            this.Controls.Add(this.BtnTriangle);
            this.Controls.Add(this.BtnSaw);
            this.Controls.Add(this.BtnRectangle);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button BtnRectangle;
        private System.Windows.Forms.Button BtnSaw;
        private System.Windows.Forms.Button BtnTriangle;
        private System.Windows.Forms.Button BtnSinus;
        private System.Windows.Forms.Label ampLabel;
        private System.Windows.Forms.Label freqLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

