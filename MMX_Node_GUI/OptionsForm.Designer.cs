namespace MMX_NODE_GUI
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.harvesterTabPage = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numThreadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.reloadInvervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.desktopTabPage = new System.Windows.Forms.TabPage();
            this.showInNotifitationGroupBox = new System.Windows.Forms.GroupBox();
            this.minimizeToNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.closeToNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.startMinimizedCheckBox = new System.Windows.Forms.CheckBox();
            this.startOnStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.confirmationOnExitCheckBox = new System.Windows.Forms.CheckBox();
            this.showInNotifitationCheckBox = new System.Windows.Forms.CheckBox();
            this.powerManagenetTabPage = new System.Windows.Forms.TabPage();
            this.inhibitSystemSleepCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.harvesterTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreadsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadInvervalNumericUpDown)).BeginInit();
            this.desktopTabPage.SuspendLayout();
            this.showInNotifitationGroupBox.SuspendLayout();
            this.powerManagenetTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(397, 326);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(316, 326);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.harvesterTabPage);
            this.tabControl1.Controls.Add(this.desktopTabPage);
            this.tabControl1.Controls.Add(this.powerManagenetTabPage);
            this.tabControl1.Location = new System.Drawing.Point(6, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 308);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // harvesterTabPage
            // 
            this.harvesterTabPage.Controls.Add(this.button2);
            this.harvesterTabPage.Controls.Add(this.button1);
            this.harvesterTabPage.Controls.Add(this.listBox1);
            this.harvesterTabPage.Controls.Add(this.label2);
            this.harvesterTabPage.Controls.Add(this.numThreadsNumericUpDown);
            this.harvesterTabPage.Controls.Add(this.label1);
            this.harvesterTabPage.Controls.Add(this.reloadInvervalNumericUpDown);
            this.harvesterTabPage.Location = new System.Drawing.Point(4, 22);
            this.harvesterTabPage.Name = "harvesterTabPage";
            this.harvesterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.harvesterTabPage.Size = new System.Drawing.Size(464, 282);
            this.harvesterTabPage.TabIndex = 2;
            this.harvesterTabPage.Text = "Harvester";
            this.harvesterTabPage.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(9, 97);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(368, 173);
            this.listBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of threads:";
            // 
            // numThreadsNumericUpDown
            // 
            this.numThreadsNumericUpDown.Location = new System.Drawing.Point(118, 32);
            this.numThreadsNumericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numThreadsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreadsNumericUpDown.Name = "numThreadsNumericUpDown";
            this.numThreadsNumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.numThreadsNumericUpDown.TabIndex = 2;
            this.numThreadsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reload interval (min):";
            // 
            // reloadInvervalNumericUpDown
            // 
            this.reloadInvervalNumericUpDown.Location = new System.Drawing.Point(118, 6);
            this.reloadInvervalNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.reloadInvervalNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.reloadInvervalNumericUpDown.Name = "reloadInvervalNumericUpDown";
            this.reloadInvervalNumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.reloadInvervalNumericUpDown.TabIndex = 0;
            // 
            // desktopTabPage
            // 
            this.desktopTabPage.Controls.Add(this.showInNotifitationGroupBox);
            this.desktopTabPage.Controls.Add(this.startMinimizedCheckBox);
            this.desktopTabPage.Controls.Add(this.startOnStartupCheckBox);
            this.desktopTabPage.Controls.Add(this.confirmationOnExitCheckBox);
            this.desktopTabPage.Controls.Add(this.showInNotifitationCheckBox);
            this.desktopTabPage.Location = new System.Drawing.Point(4, 22);
            this.desktopTabPage.Name = "desktopTabPage";
            this.desktopTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.desktopTabPage.Size = new System.Drawing.Size(464, 282);
            this.desktopTabPage.TabIndex = 0;
            this.desktopTabPage.Text = "Desktop";
            this.desktopTabPage.UseVisualStyleBackColor = true;
            // 
            // showInNotifitationGroupBox
            // 
            this.showInNotifitationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showInNotifitationGroupBox.Controls.Add(this.minimizeToNotificationCheckBox);
            this.showInNotifitationGroupBox.Controls.Add(this.closeToNotificationCheckBox);
            this.showInNotifitationGroupBox.Location = new System.Drawing.Point(6, 75);
            this.showInNotifitationGroupBox.Name = "showInNotifitationGroupBox";
            this.showInNotifitationGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.showInNotifitationGroupBox.Size = new System.Drawing.Size(452, 80);
            this.showInNotifitationGroupBox.TabIndex = 15;
            this.showInNotifitationGroupBox.TabStop = false;
            // 
            // minimizeToNotificationCheckBox
            // 
            this.minimizeToNotificationCheckBox.AutoSize = true;
            this.minimizeToNotificationCheckBox.Location = new System.Drawing.Point(9, 22);
            this.minimizeToNotificationCheckBox.Name = "minimizeToNotificationCheckBox";
            this.minimizeToNotificationCheckBox.Size = new System.Drawing.Size(156, 17);
            this.minimizeToNotificationCheckBox.TabIndex = 9;
            this.minimizeToNotificationCheckBox.Text = "Minimize to notification area";
            this.minimizeToNotificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // closeToNotificationCheckBox
            // 
            this.closeToNotificationCheckBox.AutoSize = true;
            this.closeToNotificationCheckBox.Location = new System.Drawing.Point(9, 45);
            this.closeToNotificationCheckBox.Name = "closeToNotificationCheckBox";
            this.closeToNotificationCheckBox.Size = new System.Drawing.Size(142, 17);
            this.closeToNotificationCheckBox.TabIndex = 8;
            this.closeToNotificationCheckBox.Text = "Close to notification area";
            this.closeToNotificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // startMinimizedCheckBox
            // 
            this.startMinimizedCheckBox.AutoSize = true;
            this.startMinimizedCheckBox.Location = new System.Drawing.Point(6, 29);
            this.startMinimizedCheckBox.Name = "startMinimizedCheckBox";
            this.startMinimizedCheckBox.Size = new System.Drawing.Size(96, 17);
            this.startMinimizedCheckBox.TabIndex = 14;
            this.startMinimizedCheckBox.Text = "Start minimized";
            this.startMinimizedCheckBox.UseVisualStyleBackColor = true;
            // 
            // startOnStartupCheckBox
            // 
            this.startOnStartupCheckBox.AutoSize = true;
            this.startOnStartupCheckBox.Location = new System.Drawing.Point(6, 6);
            this.startOnStartupCheckBox.Name = "startOnStartupCheckBox";
            this.startOnStartupCheckBox.Size = new System.Drawing.Size(148, 17);
            this.startOnStartupCheckBox.TabIndex = 13;
            this.startOnStartupCheckBox.Text = "Start on Windows start up";
            this.startOnStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // confirmationOnExitCheckBox
            // 
            this.confirmationOnExitCheckBox.AutoSize = true;
            this.confirmationOnExitCheckBox.Location = new System.Drawing.Point(6, 52);
            this.confirmationOnExitCheckBox.Name = "confirmationOnExitCheckBox";
            this.confirmationOnExitCheckBox.Size = new System.Drawing.Size(118, 17);
            this.confirmationOnExitCheckBox.TabIndex = 12;
            this.confirmationOnExitCheckBox.Text = "Confirmation on exit";
            this.confirmationOnExitCheckBox.UseVisualStyleBackColor = true;
            // 
            // showInNotifitationCheckBox
            // 
            this.showInNotifitationCheckBox.AutoSize = true;
            this.showInNotifitationCheckBox.Location = new System.Drawing.Point(234, 52);
            this.showInNotifitationCheckBox.Name = "showInNotifitationCheckBox";
            this.showInNotifitationCheckBox.Size = new System.Drawing.Size(142, 17);
            this.showInNotifitationCheckBox.TabIndex = 16;
            this.showInNotifitationCheckBox.Text = "Show in notification area";
            this.showInNotifitationCheckBox.UseVisualStyleBackColor = true;
            // 
            // powerManagenetTabPage
            // 
            this.powerManagenetTabPage.Controls.Add(this.inhibitSystemSleepCheckBox);
            this.powerManagenetTabPage.Location = new System.Drawing.Point(4, 22);
            this.powerManagenetTabPage.Name = "powerManagenetTabPage";
            this.powerManagenetTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.powerManagenetTabPage.Size = new System.Drawing.Size(464, 282);
            this.powerManagenetTabPage.TabIndex = 1;
            this.powerManagenetTabPage.Text = "Power Management";
            this.powerManagenetTabPage.UseVisualStyleBackColor = true;
            // 
            // inhibitSystemSleepCheckBox
            // 
            this.inhibitSystemSleepCheckBox.AutoSize = true;
            this.inhibitSystemSleepCheckBox.Location = new System.Drawing.Point(8, 6);
            this.inhibitSystemSleepCheckBox.Name = "inhibitSystemSleepCheckBox";
            this.inhibitSystemSleepCheckBox.Size = new System.Drawing.Size(117, 17);
            this.inhibitSystemSleepCheckBox.TabIndex = 12;
            this.inhibitSystemSleepCheckBox.Text = "Inhibit system sleep";
            this.inhibitSystemSleepCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(383, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Add...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(383, 126);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.harvesterTabPage.ResumeLayout(false);
            this.harvesterTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreadsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadInvervalNumericUpDown)).EndInit();
            this.desktopTabPage.ResumeLayout(false);
            this.desktopTabPage.PerformLayout();
            this.showInNotifitationGroupBox.ResumeLayout(false);
            this.showInNotifitationGroupBox.PerformLayout();
            this.powerManagenetTabPage.ResumeLayout(false);
            this.powerManagenetTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage desktopTabPage;
        private System.Windows.Forms.TabPage powerManagenetTabPage;
        private System.Windows.Forms.GroupBox showInNotifitationGroupBox;
        private System.Windows.Forms.CheckBox minimizeToNotificationCheckBox;
        private System.Windows.Forms.CheckBox closeToNotificationCheckBox;
        private System.Windows.Forms.CheckBox startMinimizedCheckBox;
        private System.Windows.Forms.CheckBox startOnStartupCheckBox;
        private System.Windows.Forms.CheckBox confirmationOnExitCheckBox;
        private System.Windows.Forms.CheckBox showInNotifitationCheckBox;
        private System.Windows.Forms.CheckBox inhibitSystemSleepCheckBox;
        private System.Windows.Forms.TabPage harvesterTabPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numThreadsNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown reloadInvervalNumericUpDown;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}