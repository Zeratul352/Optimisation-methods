namespace Optimisation_base
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Graphic = new System.Windows.Forms.PictureBox();
            this.LineCountBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DeltaXBox = new System.Windows.Forms.TextBox();
            this.DeltaYBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textlabel6 = new System.Windows.Forms.Label();
            this.CentreXBox = new System.Windows.Forms.TextBox();
            this.CentreYBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.Zoombar = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.PointX = new System.Windows.Forms.TextBox();
            this.PointY = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.picZoom = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Graphic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Zoombar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // Graphic
            // 
            this.Graphic.BackColor = System.Drawing.SystemColors.Window;
            this.Graphic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Graphic.Location = new System.Drawing.Point(12, 9);
            this.Graphic.Name = "Graphic";
            this.Graphic.Size = new System.Drawing.Size(601, 601);
            this.Graphic.TabIndex = 0;
            this.Graphic.TabStop = false;
            this.Graphic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Graphic_MouseClick);
            // 
            // LineCountBox
            // 
            this.LineCountBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LineCountBox.Location = new System.Drawing.Point(1008, 38);
            this.LineCountBox.Name = "LineCountBox";
            this.LineCountBox.Size = new System.Drawing.Size(180, 32);
            this.LineCountBox.TabIndex = 1;
            this.LineCountBox.Text = "10";
            this.LineCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(1009, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "How many lines?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(1004, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "delta X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(1128, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "delta Y";
            // 
            // DeltaXBox
            // 
            this.DeltaXBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeltaXBox.Location = new System.Drawing.Point(1008, 113);
            this.DeltaXBox.Name = "DeltaXBox";
            this.DeltaXBox.Size = new System.Drawing.Size(68, 26);
            this.DeltaXBox.TabIndex = 5;
            this.DeltaXBox.Text = "100";
            this.DeltaXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DeltaYBox
            // 
            this.DeltaYBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeltaYBox.Location = new System.Drawing.Point(1120, 114);
            this.DeltaYBox.Name = "DeltaYBox";
            this.DeltaYBox.Size = new System.Drawing.Size(68, 26);
            this.DeltaYBox.TabIndex = 6;
            this.DeltaYBox.Text = "100";
            this.DeltaYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DeltaYBox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(1004, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Centre X";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // textlabel6
            // 
            this.textlabel6.AutoSize = true;
            this.textlabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textlabel6.Location = new System.Drawing.Point(1114, 160);
            this.textlabel6.Name = "textlabel6";
            this.textlabel6.Size = new System.Drawing.Size(74, 20);
            this.textlabel6.TabIndex = 8;
            this.textlabel6.Text = "Centre Y";
            // 
            // CentreXBox
            // 
            this.CentreXBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CentreXBox.Location = new System.Drawing.Point(1008, 183);
            this.CentreXBox.Name = "CentreXBox";
            this.CentreXBox.Size = new System.Drawing.Size(68, 26);
            this.CentreXBox.TabIndex = 9;
            this.CentreXBox.Text = "0";
            this.CentreXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CentreYBox
            // 
            this.CentreYBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CentreYBox.Location = new System.Drawing.Point(1120, 183);
            this.CentreYBox.Name = "CentreYBox";
            this.CentreYBox.Size = new System.Drawing.Size(68, 26);
            this.CentreYBox.TabIndex = 10;
            this.CentreYBox.Text = "0";
            this.CentreYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(1008, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 42);
            this.button1.TabIndex = 11;
            this.button1.Text = "Draw grid";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(1008, 298);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 39);
            this.button2.TabIndex = 12;
            this.button2.Text = "Draw lines";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Clear
            // 
            this.Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Clear.Location = new System.Drawing.Point(1008, 365);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(180, 39);
            this.Clear.TabIndex = 13;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.button3_Click);
            // 
            // Zoombar
            // 
            this.Zoombar.Location = new System.Drawing.Point(1008, 410);
            this.Zoombar.Minimum = 1;
            this.Zoombar.Name = "Zoombar";
            this.Zoombar.Size = new System.Drawing.Size(180, 56);
            this.Zoombar.TabIndex = 14;
            this.Zoombar.Value = 1;
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(1008, 457);
            this.trackBar2.Maximum = 15;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(180, 56);
            this.trackBar2.TabIndex = 15;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // PointX
            // 
            this.PointX.Location = new System.Drawing.Point(1008, 530);
            this.PointX.Name = "PointX";
            this.PointX.Size = new System.Drawing.Size(61, 22);
            this.PointX.TabIndex = 16;
            this.PointX.Text = "0";
            // 
            // PointY
            // 
            this.PointY.Location = new System.Drawing.Point(1127, 530);
            this.PointY.Name = "PointY";
            this.PointY.Size = new System.Drawing.Size(61, 22);
            this.PointY.TabIndex = 17;
            this.PointY.Text = "0";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(1008, 574);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(180, 36);
            this.button3.TabIndex = 19;
            this.button3.Text = "CalcMin";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(1008, 616);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(180, 36);
            this.button4.TabIndex = 20;
            this.button4.Text = "Save";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // picZoom
            // 
            this.picZoom.BackColor = System.Drawing.SystemColors.Window;
            this.picZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picZoom.Location = new System.Drawing.Point(620, 9);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(350, 350);
            this.picZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picZoom.TabIndex = 21;
            this.picZoom.TabStop = false;
            this.picZoom.Click += new System.EventHandler(this.picZoom_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1200, 663);
            this.Controls.Add(this.picZoom);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.PointY);
            this.Controls.Add(this.PointX);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.Zoombar);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CentreYBox);
            this.Controls.Add(this.CentreXBox);
            this.Controls.Add(this.textlabel6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeltaYBox);
            this.Controls.Add(this.DeltaXBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LineCountBox);
            this.Controls.Add(this.Graphic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drawer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Graphic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Zoombar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Graphic;
        private System.Windows.Forms.TextBox LineCountBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DeltaXBox;
        private System.Windows.Forms.TextBox DeltaYBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label textlabel6;
        private System.Windows.Forms.TextBox CentreXBox;
        private System.Windows.Forms.TextBox CentreYBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.TrackBar Zoombar;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TextBox PointX;
        private System.Windows.Forms.TextBox PointY;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox picZoom;
    }
}

