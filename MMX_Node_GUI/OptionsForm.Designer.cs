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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.minimizeToNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.closeToNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.startMinimizedCheckBox = new System.Windows.Forms.CheckBox();
            this.startOnStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.confirmationOnExitCheckBox = new System.Windows.Forms.CheckBox();
            this.showInNotifitationCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.startMinimizedCheckBox);
            this.groupBox1.Controls.Add(this.startOnStartupCheckBox);
            this.groupBox1.Controls.Add(this.confirmationOnExitCheckBox);
            this.groupBox1.Controls.Add(this.showInNotifitationCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(760, 181);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Desktop";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.minimizeToNotificationCheckBox);
            this.groupBox2.Controls.Add(this.closeToNotificationCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(9, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(742, 68);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
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
            this.startMinimizedCheckBox.Location = new System.Drawing.Point(9, 45);
            this.startMinimizedCheckBox.Name = "startMinimizedCheckBox";
            this.startMinimizedCheckBox.Size = new System.Drawing.Size(96, 17);
            this.startMinimizedCheckBox.TabIndex = 4;
            this.startMinimizedCheckBox.Text = "Start minimized";
            this.startMinimizedCheckBox.UseVisualStyleBackColor = true;
            // 
            // startOnStartupCheckBox
            // 
            this.startOnStartupCheckBox.AutoSize = true;
            this.startOnStartupCheckBox.Location = new System.Drawing.Point(9, 22);
            this.startOnStartupCheckBox.Name = "startOnStartupCheckBox";
            this.startOnStartupCheckBox.Size = new System.Drawing.Size(148, 17);
            this.startOnStartupCheckBox.TabIndex = 3;
            this.startOnStartupCheckBox.Text = "Start on Windows start up";
            this.startOnStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // confirmationOnExitCheckBox
            // 
            this.confirmationOnExitCheckBox.AutoSize = true;
            this.confirmationOnExitCheckBox.Location = new System.Drawing.Point(9, 68);
            this.confirmationOnExitCheckBox.Name = "confirmationOnExitCheckBox";
            this.confirmationOnExitCheckBox.Size = new System.Drawing.Size(118, 17);
            this.confirmationOnExitCheckBox.TabIndex = 2;
            this.confirmationOnExitCheckBox.Text = "Confirmation on exit";
            this.confirmationOnExitCheckBox.UseVisualStyleBackColor = true;
            // 
            // showInNotifitationCheckBox
            // 
            this.showInNotifitationCheckBox.AutoSize = true;
            this.showInNotifitationCheckBox.Location = new System.Drawing.Point(297, 68);
            this.showInNotifitationCheckBox.Name = "showInNotifitationCheckBox";
            this.showInNotifitationCheckBox.Size = new System.Drawing.Size(142, 17);
            this.showInNotifitationCheckBox.TabIndex = 11;
            this.showInNotifitationCheckBox.Text = "Show in notification area";
            this.showInNotifitationCheckBox.UseVisualStyleBackColor = true;
            this.showInNotifitationCheckBox.CheckedChanged += new System.EventHandler(this.showInNotifitationCheckBox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(697, 426);
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
            this.okButton.Location = new System.Drawing.Point(616, 426);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox confirmationOnExitCheckBox;
        private System.Windows.Forms.CheckBox startMinimizedCheckBox;
        private System.Windows.Forms.CheckBox startOnStartupCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox minimizeToNotificationCheckBox;
        private System.Windows.Forms.CheckBox closeToNotificationCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox showInNotifitationCheckBox;
    }
}