namespace appsizerGUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.winlist = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.abovetaskbar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.window = new System.Windows.Forms.ComboBox();
            this.b = new System.Windows.Forms.Label();
            this.r = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.h = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.w = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.y = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.x = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.calibrate = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.winlist,
            this.toolStripMenuItem7,
            this.toolStripMenuItem9,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(329, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // winlist
            // 
            this.winlist.Name = "winlist";
            this.winlist.Size = new System.Drawing.Size(95, 20);
            this.winlist.Text = "Select window";
            this.winlist.DropDownOpening += new System.EventHandler(this.listWin);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abovetaskbar});
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItem7.Text = "Options";
            // 
            // abovetaskbar
            // 
            this.abovetaskbar.Checked = true;
            this.abovetaskbar.CheckOnClick = true;
            this.abovetaskbar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.abovetaskbar.Name = "abovetaskbar";
            this.abovetaskbar.Size = new System.Drawing.Size(230, 22);
            this.abovetaskbar.Text = "Center window above taskbar";
            this.abovetaskbar.Click += new System.EventHandler(this.toggleAboveTaskbar);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(43, 20);
            this.toolStripMenuItem9.Text = "Save";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.saveWin);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(62, 20);
            this.toolStripMenuItem1.Text = "Remove";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.removeWin);
            // 
            // window
            // 
            this.window.FormattingEnabled = true;
            this.window.Location = new System.Drawing.Point(12, 30);
            this.window.Name = "window";
            this.window.Size = new System.Drawing.Size(305, 21);
            this.window.TabIndex = 0;
            this.window.Text = "Type or select a window by title...";
            this.window.DropDown += new System.EventHandler(this.listSavedWin);
            this.window.SelectedIndexChanged += new System.EventHandler(this.loadSavedWin);
            // 
            // b
            // 
            this.b.AutoSize = true;
            this.b.Location = new System.Drawing.Point(282, 90);
            this.b.Name = "b";
            this.b.Size = new System.Drawing.Size(13, 13);
            this.b.TabIndex = 25;
            this.b.Text = "0";
            // 
            // r
            // 
            this.r.AutoSize = true;
            this.r.Location = new System.Drawing.Point(282, 64);
            this.r.Name = "r";
            this.r.Size = new System.Drawing.Size(13, 13);
            this.r.TabIndex = 26;
            this.r.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Bottom:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Right:";
            // 
            // h
            // 
            this.h.Location = new System.Drawing.Point(163, 88);
            this.h.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.h.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.h.Name = "h";
            this.h.Size = new System.Drawing.Size(60, 20);
            this.h.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Height:";
            // 
            // w
            // 
            this.w.Location = new System.Drawing.Point(163, 62);
            this.w.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.w.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.w.Name = "w";
            this.w.Size = new System.Drawing.Size(60, 20);
            this.w.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Width:";
            // 
            // y
            // 
            this.y.Location = new System.Drawing.Point(35, 88);
            this.y.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.y.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.y.Name = "y";
            this.y.Size = new System.Drawing.Size(60, 20);
            this.y.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Y:";
            // 
            // x
            // 
            this.x.Location = new System.Drawing.Point(35, 62);
            this.x.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.x.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.x.Name = "x";
            this.x.Size = new System.Drawing.Size(60, 20);
            this.x.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "X:";
            // 
            // calibrate
            // 
            this.calibrate.AutoSize = true;
            this.calibrate.Checked = true;
            this.calibrate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.calibrate.Location = new System.Drawing.Point(12, 118);
            this.calibrate.Name = "calibrate";
            this.calibrate.Size = new System.Drawing.Size(288, 17);
            this.calibrate.TabIndex = 5;
            this.calibrate.Text = "Use -7 pixel calibration (useful for native window border)";
            this.calibrate.UseVisualStyleBackColor = true;
            this.calibrate.CheckedChanged += new System.EventHandler(this.toggleCalibrate);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.refreshPos);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(134, 142);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Center";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.centerWin);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(214, 142);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Apply";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.setPos);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 175);
            this.Controls.Add(this.calibrate);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.b);
            this.Controls.Add(this.r);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.h);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.w);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.y);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.x);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.window);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "appsizerGUI";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ComboBox window;
        private System.Windows.Forms.Label b;
        private System.Windows.Forms.Label r;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown h;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown w;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown y;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown x;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem abovetaskbar;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem winlist;
        private System.Windows.Forms.CheckBox calibrate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

