namespace appsizerGUI
{
    partial class appsizerGUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuWindowSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.dummyWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDesktopNewProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRestoreDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDesktopProfileManage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.useAboveTaskbar = new System.Windows.Forms.ToolStripMenuItem();
            this.savedWindowSelector = new System.Windows.Forms.ComboBox();
            this.windowBottom = new System.Windows.Forms.Label();
            this.windowRight = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.windowHeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.windowWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.windowY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.windowX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnWindowTools = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSaveWindow = new System.Windows.Forms.Button();
            this.btnRemoveWindow = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.windowToolsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolsAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolsShowWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolsStyleSeparatorStart = new System.Windows.Forms.ToolStripSeparator();
            this.windowToolsStyleSeparatorEnd = new System.Windows.Forms.ToolStripSeparator();
            this.windowToolsBorder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindowBorderSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowX)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.windowToolsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowSelect,
            this.toolStripMenuItem1,
            this.menuOptions});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(329, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuWindowSelect
            // 
            this.menuWindowSelect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyWindowToolStripMenuItem});
            this.menuWindowSelect.Name = "menuWindowSelect";
            this.menuWindowSelect.Size = new System.Drawing.Size(95, 20);
            this.menuWindowSelect.Text = "&Select window";
            this.menuWindowSelect.DropDownOpening += new System.EventHandler(this.ListWindows);
            // 
            // dummyWindowToolStripMenuItem
            // 
            this.dummyWindowToolStripMenuItem.Name = "dummyWindowToolStripMenuItem";
            this.dummyWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dummyWindowToolStripMenuItem.Text = "Dummy window";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveDesktop,
            this.menuRestoreDesktop,
            this.menuDesktopProfileManage});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(62, 20);
            this.toolStripMenuItem1.Text = "&Desktop";
            this.toolStripMenuItem1.DropDownOpening += new System.EventHandler(this.OnListDesktopProfiles);
            // 
            // menuSaveDesktop
            // 
            this.menuSaveDesktop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveDesktopNewProfile});
            this.menuSaveDesktop.Name = "menuSaveDesktop";
            this.menuSaveDesktop.Size = new System.Drawing.Size(238, 22);
            this.menuSaveDesktop.Text = "Save all open window as";
            // 
            // menuSaveDesktopNewProfile
            // 
            this.menuSaveDesktopNewProfile.Name = "menuSaveDesktopNewProfile";
            this.menuSaveDesktopNewProfile.Size = new System.Drawing.Size(180, 22);
            this.menuSaveDesktopNewProfile.Text = "< New profile >";
            this.menuSaveDesktopNewProfile.Click += new System.EventHandler(this.OnAddDesktopProfileClick);
            // 
            // menuRestoreDesktop
            // 
            this.menuRestoreDesktop.Name = "menuRestoreDesktop";
            this.menuRestoreDesktop.Size = new System.Drawing.Size(238, 22);
            this.menuRestoreDesktop.Text = "Restore window positions from";
            // 
            // menuDesktopProfileManage
            // 
            this.menuDesktopProfileManage.Name = "menuDesktopProfileManage";
            this.menuDesktopProfileManage.Size = new System.Drawing.Size(238, 22);
            this.menuDesktopProfileManage.Text = "Manage profiles";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useAboveTaskbar,
            this.menuWindowBorderSelect});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // useAboveTaskbar
            // 
            this.useAboveTaskbar.Checked = true;
            this.useAboveTaskbar.CheckOnClick = true;
            this.useAboveTaskbar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAboveTaskbar.Name = "useAboveTaskbar";
            this.useAboveTaskbar.Size = new System.Drawing.Size(262, 22);
            this.useAboveTaskbar.Text = "Put centered window above taskbar";
            // 
            // savedWindowSelector
            // 
            this.savedWindowSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savedWindowSelector.FormattingEnabled = true;
            this.savedWindowSelector.Location = new System.Drawing.Point(12, 30);
            this.savedWindowSelector.Name = "savedWindowSelector";
            this.savedWindowSelector.Size = new System.Drawing.Size(254, 21);
            this.savedWindowSelector.TabIndex = 0;
            this.savedWindowSelector.Text = "Select a window...";
            this.savedWindowSelector.DropDown += new System.EventHandler(this.ListSavedWindows);
            this.savedWindowSelector.SelectedIndexChanged += new System.EventHandler(this.LoadSavedWindow);
            // 
            // windowBottom
            // 
            this.windowBottom.AutoSize = true;
            this.windowBottom.Location = new System.Drawing.Point(282, 90);
            this.windowBottom.Name = "windowBottom";
            this.windowBottom.Size = new System.Drawing.Size(13, 13);
            this.windowBottom.TabIndex = 25;
            this.windowBottom.Text = "0";
            // 
            // windowRight
            // 
            this.windowRight.AutoSize = true;
            this.windowRight.Location = new System.Drawing.Point(282, 64);
            this.windowRight.Name = "windowRight";
            this.windowRight.Size = new System.Drawing.Size(13, 13);
            this.windowRight.TabIndex = 26;
            this.windowRight.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Bottom:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Right:";
            // 
            // windowHeight
            // 
            this.windowHeight.Location = new System.Drawing.Point(163, 88);
            this.windowHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.windowHeight.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.windowHeight.Name = "windowHeight";
            this.windowHeight.Size = new System.Drawing.Size(60, 22);
            this.windowHeight.TabIndex = 6;
            this.windowHeight.ValueChanged += new System.EventHandler(this.OnSetPosition);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Height:";
            // 
            // windowWidth
            // 
            this.windowWidth.Location = new System.Drawing.Point(163, 62);
            this.windowWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.windowWidth.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.windowWidth.Name = "windowWidth";
            this.windowWidth.Size = new System.Drawing.Size(60, 22);
            this.windowWidth.TabIndex = 5;
            this.windowWidth.ValueChanged += new System.EventHandler(this.OnSetPosition);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Width:";
            // 
            // windowY
            // 
            this.windowY.Location = new System.Drawing.Point(35, 88);
            this.windowY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.windowY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.windowY.Name = "windowY";
            this.windowY.Size = new System.Drawing.Size(60, 22);
            this.windowY.TabIndex = 4;
            this.windowY.ValueChanged += new System.EventHandler(this.OnSetPosition);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Y:";
            // 
            // windowX
            // 
            this.windowX.Location = new System.Drawing.Point(35, 62);
            this.windowX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.windowX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.windowX.Name = "windowX";
            this.windowX.Size = new System.Drawing.Size(60, 22);
            this.windowX.TabIndex = 3;
            this.windowX.ValueChanged += new System.EventHandler(this.OnSetPosition);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "X:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(11, 117);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(60, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.OnRefreshClicked);
            // 
            // btnWindowTools
            // 
            this.btnWindowTools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowTools.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWindowTools.Location = new System.Drawing.Point(192, 117);
            this.btnWindowTools.Name = "btnWindowTools";
            this.btnWindowTools.Size = new System.Drawing.Size(60, 23);
            this.btnWindowTools.TabIndex = 9;
            this.btnWindowTools.Text = "&Tools";
            this.btnWindowTools.UseVisualStyleBackColor = true;
            this.btnWindowTools.Click += new System.EventHandler(this.OnWindowToolsClick);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(258, 117);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(60, 23);
            this.btnApply.TabIndex = 10;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnSetPosition);
            // 
            // btnSaveWindow
            // 
            this.btnSaveWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveWindow.Location = new System.Drawing.Point(270, 29);
            this.btnSaveWindow.Name = "btnSaveWindow";
            this.btnSaveWindow.Size = new System.Drawing.Size(23, 23);
            this.btnSaveWindow.TabIndex = 1;
            this.btnSaveWindow.Text = "+";
            this.btnSaveWindow.UseVisualStyleBackColor = true;
            this.btnSaveWindow.Click += new System.EventHandler(this.SaveCurrentWindow);
            // 
            // btnRemoveWindow
            // 
            this.btnRemoveWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveWindow.Location = new System.Drawing.Point(295, 29);
            this.btnRemoveWindow.Name = "btnRemoveWindow";
            this.btnRemoveWindow.Size = new System.Drawing.Size(23, 23);
            this.btnRemoveWindow.TabIndex = 2;
            this.btnRemoveWindow.Text = "-";
            this.btnRemoveWindow.UseVisualStyleBackColor = true;
            this.btnRemoveWindow.Click += new System.EventHandler(this.RemoveCurrentWindow);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 147);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(329, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // windowToolsMenu
            // 
            this.windowToolsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.windowToolsAlwaysOnTop,
            this.windowToolsShowWindow,
            this.windowToolsStyleSeparatorStart,
            this.windowToolsStyleSeparatorEnd,
            this.windowToolsBorder});
            this.windowToolsMenu.Name = "windowToolsMenu";
            this.windowToolsMenu.Size = new System.Drawing.Size(183, 126);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(182, 22);
            this.toolStripMenuItem3.Text = "Put to center";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.OnCenterClicked);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(182, 22);
            this.toolStripMenuItem4.Text = "Borderless fullscreen";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.OnBorderlessClicked);
            // 
            // windowToolsAlwaysOnTop
            // 
            this.windowToolsAlwaysOnTop.CheckOnClick = true;
            this.windowToolsAlwaysOnTop.Name = "windowToolsAlwaysOnTop";
            this.windowToolsAlwaysOnTop.Size = new System.Drawing.Size(182, 22);
            this.windowToolsAlwaysOnTop.Text = "Always on top";
            this.windowToolsAlwaysOnTop.Click += new System.EventHandler(this.OnAlwaysOnTopClicked);
            // 
            // windowToolsShowWindow
            // 
            this.windowToolsShowWindow.Name = "windowToolsShowWindow";
            this.windowToolsShowWindow.Size = new System.Drawing.Size(182, 22);
            this.windowToolsShowWindow.Text = "ShowWindow";
            // 
            // windowToolsStyleSeparatorStart
            // 
            this.windowToolsStyleSeparatorStart.Name = "windowToolsStyleSeparatorStart";
            this.windowToolsStyleSeparatorStart.Size = new System.Drawing.Size(179, 6);
            // 
            // windowToolsStyleSeparatorEnd
            // 
            this.windowToolsStyleSeparatorEnd.Name = "windowToolsStyleSeparatorEnd";
            this.windowToolsStyleSeparatorEnd.Size = new System.Drawing.Size(179, 6);
            // 
            // windowToolsBorder
            // 
            this.windowToolsBorder.Enabled = false;
            this.windowToolsBorder.Name = "windowToolsBorder";
            this.windowToolsBorder.Size = new System.Drawing.Size(182, 22);
            this.windowToolsBorder.Text = "Border: (-, -, -, -)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // menuWindowBorderSelect
            // 
            this.menuWindowBorderSelect.Name = "menuWindowBorderSelect";
            this.menuWindowBorderSelect.Size = new System.Drawing.Size(262, 22);
            this.menuWindowBorderSelect.Text = "Window border calculation method";
            // 
            // appsizerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(329, 169);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnRemoveWindow);
            this.Controls.Add(this.btnSaveWindow);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnWindowTools);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.windowBottom);
            this.Controls.Add(this.windowRight);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.windowHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.windowWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.windowY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.windowX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.savedWindowSelector);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "appsizerGUI";
            this.Text = "appsizerGUI";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowX)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.windowToolsMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ComboBox savedWindowSelector;
        private System.Windows.Forms.Label windowBottom;
        private System.Windows.Forms.Label windowRight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown windowHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown windowWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown windowY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown windowX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem useAboveTaskbar;
        private System.Windows.Forms.ToolStripMenuItem menuWindowSelect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnWindowTools;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSaveWindow;
        private System.Windows.Forms.Button btnRemoveWindow;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDesktop;
        private System.Windows.Forms.ToolStripMenuItem menuRestoreDesktop;
        private System.Windows.Forms.ContextMenuStrip windowToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator windowToolsStyleSeparatorStart;
        private System.Windows.Forms.ToolStripMenuItem windowToolsBorder;
        private System.Windows.Forms.ToolStripSeparator windowToolsStyleSeparatorEnd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem windowToolsAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem dummyWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDesktopNewProfile;
        private System.Windows.Forms.ToolStripMenuItem menuDesktopProfileManage;
        private System.Windows.Forms.ToolStripMenuItem windowToolsShowWindow;
        private System.Windows.Forms.ToolStripMenuItem menuWindowBorderSelect;
    }
}

