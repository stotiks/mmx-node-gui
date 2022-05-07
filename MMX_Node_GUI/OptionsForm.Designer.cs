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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.plotDirListBox = new System.Windows.Forms.ListBox();
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
            this.tabControl1.SuspendLayout();
            this.harvesterTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.harvesterTabPage.Controls.Add(this.groupBox1);
            this.harvesterTabPage.Location = new System.Drawing.Point(4, 22);
            this.harvesterTabPage.Name = "harvesterTabPage";
            this.harvesterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.harvesterTabPage.Size = new System.Drawing.Size(464, 282);
            this.harvesterTabPage.TabIndex = 2;
            this.harvesterTabPage.Text = "Harvester";
            this.harvesterTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.removeButton);
            this.groupBox1.Controls.Add(this.addButton);
            this.groupBox1.Controls.Add(this.plotDirListBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 276);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load plots from (node restart required):";
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(378, 48);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 9;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(378, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add...";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // plotDirListBox
            // 
            this.plotDirListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotDirListBox.FormattingEnabled = true;
            this.plotDirListBox.Location = new System.Drawing.Point(5, 19);
            this.plotDirListBox.Name = "plotDirListBox";
            this.plotDirListBox.Size = new System.Drawing.Size(368, 251);
            this.plotDirListBox.TabIndex = 7;
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
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ListBox plotDirListBox;
    }
}