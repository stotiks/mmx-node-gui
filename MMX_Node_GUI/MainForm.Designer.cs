
using System.Drawing;

namespace MMX_NODE_GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeTabPage = new System.Windows.Forms.TabPage();
            this.MenuMaterialTabControl = new MaterialSkin.Controls.MaterialTabControl();
            this.harvesterTabPage = new System.Windows.Forms.TabPage();
            this.harvesterPlotsMaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.removePlotFolderMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.addPlotFolderMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.plotFoldersMaterialListBox = new MaterialSkin.Controls.MaterialListBox();
            this.plotterTabPage = new System.Windows.Forms.TabPage();
            this.materialMultiLineTextBox21 = new MaterialSkin.Controls.MaterialMultiLineTextBox2();
            this.countMaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.keysTabPage = new System.Windows.Forms.TabPage();
            this.farmerkeyMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.nftplotMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.contractMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.poolkeyMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.perfomanceTabPage = new System.Windows.Forms.TabPage();
            this.buckets3MaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.bucketsMaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.rmulti2MaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.threadsMaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.rmulti2MaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.threadsMaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.buckets3MaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.bucketsMaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.directoriesTabPage = new System.Windows.Forms.TabPage();
            this.tmpdirMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.tmpdir2MaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.stagedirMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.directoutMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.stagedirMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.tmptoggleMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.tmpdirMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.waitforcopyMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.tmpdir2MaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.sizeMaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.countMaterialNumericUpDown = new MMX_NODE_GUI.MaterialNumericUpDown();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.startMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.finaldirMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.sizeMaterialLabel = new MaterialSkin.Controls.MaterialLabel();
            this.finaldirMaterialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.inhibitSystemSleepMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.confirmationOnExitMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.startMinimizedMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.startOnStartupMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.showInNotifitationMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.showInNotifitationGroupBox = new System.Windows.Forms.GroupBox();
            this.closeToNotificationMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.minimizeToNotificationMaterialSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.menuIconList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip1.SuspendLayout();
            this.MenuMaterialTabControl.SuspendLayout();
            this.harvesterTabPage.SuspendLayout();
            this.plotterTabPage.SuspendLayout();
            this.materialTabControl1.SuspendLayout();
            this.keysTabPage.SuspendLayout();
            this.perfomanceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rmulti2MaterialNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsMaterialNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buckets3MaterialNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bucketsMaterialNumericUpDown)).BeginInit();
            this.directoriesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMaterialNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countMaterialNumericUpDown)).BeginInit();
            this.settingsTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.showInNotifitationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MMX Node";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.toolStripSeparator6,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 54);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(100, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // nodeTabPage
            // 
            this.nodeTabPage.ImageKey = "database-sync.png";
            this.nodeTabPage.Location = new System.Drawing.Point(4, 31);
            this.nodeTabPage.Name = "nodeTabPage";
            this.nodeTabPage.Size = new System.Drawing.Size(1056, 613);
            this.nodeTabPage.TabIndex = 0;
            this.nodeTabPage.Text = "Node";
            // 
            // MenuMaterialTabControl
            // 
            this.MenuMaterialTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuMaterialTabControl.Controls.Add(this.nodeTabPage);
            this.MenuMaterialTabControl.Controls.Add(this.harvesterTabPage);
            this.MenuMaterialTabControl.Controls.Add(this.plotterTabPage);
            this.MenuMaterialTabControl.Controls.Add(this.settingsTabPage);
            this.MenuMaterialTabControl.Depth = 0;
            this.MenuMaterialTabControl.ImageList = this.menuIconList;
            this.MenuMaterialTabControl.Location = new System.Drawing.Point(3, 64);
            this.MenuMaterialTabControl.MouseState = MaterialSkin.MouseState.HOVER;
            this.MenuMaterialTabControl.Multiline = true;
            this.MenuMaterialTabControl.Name = "MenuMaterialTabControl";
            this.MenuMaterialTabControl.SelectedIndex = 0;
            this.MenuMaterialTabControl.Size = new System.Drawing.Size(1064, 648);
            this.MenuMaterialTabControl.TabIndex = 4;
            // 
            // harvesterTabPage
            // 
            this.harvesterTabPage.Controls.Add(this.harvesterPlotsMaterialLabel);
            this.harvesterTabPage.Controls.Add(this.removePlotFolderMaterialButton);
            this.harvesterTabPage.Controls.Add(this.addPlotFolderMaterialButton);
            this.harvesterTabPage.Controls.Add(this.plotFoldersMaterialListBox);
            this.harvesterTabPage.ImageKey = "tractor.png";
            this.harvesterTabPage.Location = new System.Drawing.Point(4, 31);
            this.harvesterTabPage.Name = "harvesterTabPage";
            this.harvesterTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.harvesterTabPage.Size = new System.Drawing.Size(1056, 613);
            this.harvesterTabPage.TabIndex = 3;
            this.harvesterTabPage.Text = "Harvester";
            // 
            // harvesterPlotsMaterialLabel
            // 
            this.harvesterPlotsMaterialLabel.AutoSize = true;
            this.harvesterPlotsMaterialLabel.Depth = 0;
            this.harvesterPlotsMaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.harvesterPlotsMaterialLabel.Location = new System.Drawing.Point(10, 10);
            this.harvesterPlotsMaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.harvesterPlotsMaterialLabel.Name = "harvesterPlotsMaterialLabel";
            this.harvesterPlotsMaterialLabel.Size = new System.Drawing.Size(278, 19);
            this.harvesterPlotsMaterialLabel.TabIndex = 4;
            this.harvesterPlotsMaterialLabel.Text = "Load plots from (node restart required):";
            // 
            // removePlotFolderMaterialButton
            // 
            this.removePlotFolderMaterialButton.AutoSize = false;
            this.removePlotFolderMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removePlotFolderMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.removePlotFolderMaterialButton.Depth = 0;
            this.removePlotFolderMaterialButton.HighEmphasis = false;
            this.removePlotFolderMaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder_remove;
            this.removePlotFolderMaterialButton.Location = new System.Drawing.Point(505, 80);
            this.removePlotFolderMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.removePlotFolderMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.removePlotFolderMaterialButton.Name = "removePlotFolderMaterialButton";
            this.removePlotFolderMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.removePlotFolderMaterialButton.Size = new System.Drawing.Size(111, 36);
            this.removePlotFolderMaterialButton.TabIndex = 3;
            this.removePlotFolderMaterialButton.Text = "Remove";
            this.removePlotFolderMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.removePlotFolderMaterialButton.UseAccentColor = false;
            this.removePlotFolderMaterialButton.UseVisualStyleBackColor = true;
            this.removePlotFolderMaterialButton.Click += new System.EventHandler(this.removePlotFolderMaterialButton_Click);
            // 
            // addPlotFolderMaterialButton
            // 
            this.addPlotFolderMaterialButton.AutoSize = false;
            this.addPlotFolderMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addPlotFolderMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.addPlotFolderMaterialButton.Depth = 0;
            this.addPlotFolderMaterialButton.HighEmphasis = false;
            this.addPlotFolderMaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder_plus;
            this.addPlotFolderMaterialButton.Location = new System.Drawing.Point(505, 32);
            this.addPlotFolderMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.addPlotFolderMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.addPlotFolderMaterialButton.Name = "addPlotFolderMaterialButton";
            this.addPlotFolderMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.addPlotFolderMaterialButton.Size = new System.Drawing.Size(111, 36);
            this.addPlotFolderMaterialButton.TabIndex = 2;
            this.addPlotFolderMaterialButton.Text = "Add";
            this.addPlotFolderMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.addPlotFolderMaterialButton.UseAccentColor = false;
            this.addPlotFolderMaterialButton.UseVisualStyleBackColor = true;
            this.addPlotFolderMaterialButton.Click += new System.EventHandler(this.addPlotFolderMaterialButton_Click);
            // 
            // plotFoldersMaterialListBox
            // 
            this.plotFoldersMaterialListBox.BackColor = System.Drawing.Color.White;
            this.plotFoldersMaterialListBox.BorderColor = System.Drawing.Color.LightGray;
            this.plotFoldersMaterialListBox.Depth = 0;
            this.plotFoldersMaterialListBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plotFoldersMaterialListBox.Location = new System.Drawing.Point(10, 32);
            this.plotFoldersMaterialListBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.plotFoldersMaterialListBox.Name = "plotFoldersMaterialListBox";
            this.plotFoldersMaterialListBox.SelectedIndex = -1;
            this.plotFoldersMaterialListBox.SelectedItem = null;
            this.plotFoldersMaterialListBox.Size = new System.Drawing.Size(485, 331);
            this.plotFoldersMaterialListBox.TabIndex = 1;
            // 
            // plotterTabPage
            // 
            this.plotterTabPage.Controls.Add(this.materialMultiLineTextBox21);
            this.plotterTabPage.Controls.Add(this.countMaterialLabel);
            this.plotterTabPage.Controls.Add(this.materialTabControl1);
            this.plotterTabPage.Controls.Add(this.sizeMaterialNumericUpDown);
            this.plotterTabPage.Controls.Add(this.countMaterialNumericUpDown);
            this.plotterTabPage.Controls.Add(this.materialTabSelector1);
            this.plotterTabPage.Controls.Add(this.startMaterialButton);
            this.plotterTabPage.Controls.Add(this.finaldirMaterialButton);
            this.plotterTabPage.Controls.Add(this.sizeMaterialLabel);
            this.plotterTabPage.Controls.Add(this.finaldirMaterialTextBox2);
            this.plotterTabPage.ImageKey = "harddisk-plus.png";
            this.plotterTabPage.Location = new System.Drawing.Point(4, 31);
            this.plotterTabPage.Name = "plotterTabPage";
            this.plotterTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.plotterTabPage.Size = new System.Drawing.Size(1056, 613);
            this.plotterTabPage.TabIndex = 4;
            this.plotterTabPage.Text = "Plotter";
            // 
            // materialMultiLineTextBox21
            // 
            this.materialMultiLineTextBox21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialMultiLineTextBox21.AnimateReadOnly = false;
            this.materialMultiLineTextBox21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialMultiLineTextBox21.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialMultiLineTextBox21.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialMultiLineTextBox21.Depth = 0;
            this.materialMultiLineTextBox21.HideSelection = true;
            this.materialMultiLineTextBox21.Location = new System.Drawing.Point(10, 478);
            this.materialMultiLineTextBox21.MaxLength = 32767;
            this.materialMultiLineTextBox21.MouseState = MaterialSkin.MouseState.OUT;
            this.materialMultiLineTextBox21.Name = "materialMultiLineTextBox21";
            this.materialMultiLineTextBox21.PasswordChar = '\0';
            this.materialMultiLineTextBox21.ReadOnly = true;
            this.materialMultiLineTextBox21.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.materialMultiLineTextBox21.SelectedText = "";
            this.materialMultiLineTextBox21.SelectionLength = 0;
            this.materialMultiLineTextBox21.SelectionStart = 0;
            this.materialMultiLineTextBox21.ShortcutsEnabled = true;
            this.materialMultiLineTextBox21.Size = new System.Drawing.Size(977, 122);
            this.materialMultiLineTextBox21.TabIndex = 26;
            this.materialMultiLineTextBox21.TabStop = false;
            this.materialMultiLineTextBox21.Text = "materialMultiLineTextBox21";
            this.materialMultiLineTextBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialMultiLineTextBox21.UseSystemPasswordChar = false;
            // 
            // countMaterialLabel
            // 
            this.countMaterialLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.countMaterialLabel.AutoSize = true;
            this.countMaterialLabel.Depth = 0;
            this.countMaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.countMaterialLabel.Location = new System.Drawing.Point(515, 86);
            this.countMaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.countMaterialLabel.Name = "countMaterialLabel";
            this.countMaterialLabel.Size = new System.Drawing.Size(180, 19);
            this.countMaterialLabel.TabIndex = 16;
            this.countMaterialLabel.Text = "Number of plots to create";
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabControl1.Controls.Add(this.keysTabPage);
            this.materialTabControl1.Controls.Add(this.perfomanceTabPage);
            this.materialTabControl1.Controls.Add(this.directoriesTabPage);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(10, 187);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(977, 250);
            this.materialTabControl1.TabIndex = 25;
            // 
            // keysTabPage
            // 
            this.keysTabPage.Controls.Add(this.farmerkeyMaterialTextBox2);
            this.keysTabPage.Controls.Add(this.nftplotMaterialSwitch);
            this.keysTabPage.Controls.Add(this.contractMaterialTextBox2);
            this.keysTabPage.Controls.Add(this.poolkeyMaterialTextBox2);
            this.keysTabPage.Location = new System.Drawing.Point(4, 22);
            this.keysTabPage.Name = "keysTabPage";
            this.keysTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.keysTabPage.Size = new System.Drawing.Size(969, 224);
            this.keysTabPage.TabIndex = 0;
            this.keysTabPage.Text = "Keys";
            // 
            // farmerkeyMaterialTextBox2
            // 
            this.farmerkeyMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.farmerkeyMaterialTextBox2.AnimateReadOnly = false;
            this.farmerkeyMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.farmerkeyMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.farmerkeyMaterialTextBox2.Depth = 0;
            this.farmerkeyMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.farmerkeyMaterialTextBox2.HideSelection = true;
            this.farmerkeyMaterialTextBox2.LeadingIcon = null;
            this.farmerkeyMaterialTextBox2.Location = new System.Drawing.Point(6, 6);
            this.farmerkeyMaterialTextBox2.MaxLength = 32767;
            this.farmerkeyMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.farmerkeyMaterialTextBox2.Name = "farmerkeyMaterialTextBox2";
            this.farmerkeyMaterialTextBox2.PasswordChar = '\0';
            this.farmerkeyMaterialTextBox2.PrefixSuffixText = null;
            this.farmerkeyMaterialTextBox2.ReadOnly = false;
            this.farmerkeyMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.farmerkeyMaterialTextBox2.SelectedText = "";
            this.farmerkeyMaterialTextBox2.SelectionLength = 0;
            this.farmerkeyMaterialTextBox2.SelectionStart = 0;
            this.farmerkeyMaterialTextBox2.ShortcutsEnabled = true;
            this.farmerkeyMaterialTextBox2.Size = new System.Drawing.Size(957, 48);
            this.farmerkeyMaterialTextBox2.TabIndex = 0;
            this.farmerkeyMaterialTextBox2.TabStop = false;
            this.farmerkeyMaterialTextBox2.Text = "farmerkeyMaterialTextBox2";
            this.farmerkeyMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.farmerkeyMaterialTextBox2.TrailingIcon = null;
            this.farmerkeyMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // nftplotMaterialSwitch
            // 
            this.nftplotMaterialSwitch.AutoSize = true;
            this.nftplotMaterialSwitch.Depth = 0;
            this.nftplotMaterialSwitch.Location = new System.Drawing.Point(6, 57);
            this.nftplotMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.nftplotMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.nftplotMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.nftplotMaterialSwitch.Name = "nftplotMaterialSwitch";
            this.nftplotMaterialSwitch.Ripple = true;
            this.nftplotMaterialSwitch.Size = new System.Drawing.Size(211, 37);
            this.nftplotMaterialSwitch.TabIndex = 10;
            this.nftplotMaterialSwitch.Text = "nftplotMaterialSwitch";
            this.nftplotMaterialSwitch.UseVisualStyleBackColor = true;
            // 
            // contractMaterialTextBox2
            // 
            this.contractMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contractMaterialTextBox2.AnimateReadOnly = false;
            this.contractMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contractMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.contractMaterialTextBox2.Depth = 0;
            this.contractMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.contractMaterialTextBox2.HideSelection = true;
            this.contractMaterialTextBox2.LeadingIcon = null;
            this.contractMaterialTextBox2.Location = new System.Drawing.Point(6, 157);
            this.contractMaterialTextBox2.MaxLength = 32767;
            this.contractMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.contractMaterialTextBox2.Name = "contractMaterialTextBox2";
            this.contractMaterialTextBox2.PasswordChar = '\0';
            this.contractMaterialTextBox2.PrefixSuffixText = null;
            this.contractMaterialTextBox2.ReadOnly = false;
            this.contractMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.contractMaterialTextBox2.SelectedText = "";
            this.contractMaterialTextBox2.SelectionLength = 0;
            this.contractMaterialTextBox2.SelectionStart = 0;
            this.contractMaterialTextBox2.ShortcutsEnabled = true;
            this.contractMaterialTextBox2.Size = new System.Drawing.Size(957, 48);
            this.contractMaterialTextBox2.TabIndex = 2;
            this.contractMaterialTextBox2.TabStop = false;
            this.contractMaterialTextBox2.Text = "contractMaterialTextBox2";
            this.contractMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.contractMaterialTextBox2.TrailingIcon = null;
            this.contractMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // poolkeyMaterialTextBox2
            // 
            this.poolkeyMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.poolkeyMaterialTextBox2.AnimateReadOnly = false;
            this.poolkeyMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.poolkeyMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.poolkeyMaterialTextBox2.Depth = 0;
            this.poolkeyMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.poolkeyMaterialTextBox2.HideSelection = true;
            this.poolkeyMaterialTextBox2.LeadingIcon = null;
            this.poolkeyMaterialTextBox2.Location = new System.Drawing.Point(6, 103);
            this.poolkeyMaterialTextBox2.MaxLength = 32767;
            this.poolkeyMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.poolkeyMaterialTextBox2.Name = "poolkeyMaterialTextBox2";
            this.poolkeyMaterialTextBox2.PasswordChar = '\0';
            this.poolkeyMaterialTextBox2.PrefixSuffixText = null;
            this.poolkeyMaterialTextBox2.ReadOnly = false;
            this.poolkeyMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.poolkeyMaterialTextBox2.SelectedText = "";
            this.poolkeyMaterialTextBox2.SelectionLength = 0;
            this.poolkeyMaterialTextBox2.SelectionStart = 0;
            this.poolkeyMaterialTextBox2.ShortcutsEnabled = true;
            this.poolkeyMaterialTextBox2.Size = new System.Drawing.Size(957, 48);
            this.poolkeyMaterialTextBox2.TabIndex = 1;
            this.poolkeyMaterialTextBox2.TabStop = false;
            this.poolkeyMaterialTextBox2.Text = "poolkeyMaterialTextBox2";
            this.poolkeyMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.poolkeyMaterialTextBox2.TrailingIcon = null;
            this.poolkeyMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // perfomanceTabPage
            // 
            this.perfomanceTabPage.Controls.Add(this.buckets3MaterialLabel);
            this.perfomanceTabPage.Controls.Add(this.bucketsMaterialLabel);
            this.perfomanceTabPage.Controls.Add(this.rmulti2MaterialLabel);
            this.perfomanceTabPage.Controls.Add(this.threadsMaterialLabel);
            this.perfomanceTabPage.Controls.Add(this.rmulti2MaterialNumericUpDown);
            this.perfomanceTabPage.Controls.Add(this.threadsMaterialNumericUpDown);
            this.perfomanceTabPage.Controls.Add(this.buckets3MaterialNumericUpDown);
            this.perfomanceTabPage.Controls.Add(this.bucketsMaterialNumericUpDown);
            this.perfomanceTabPage.Location = new System.Drawing.Point(4, 22);
            this.perfomanceTabPage.Name = "perfomanceTabPage";
            this.perfomanceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.perfomanceTabPage.Size = new System.Drawing.Size(969, 224);
            this.perfomanceTabPage.TabIndex = 2;
            this.perfomanceTabPage.Text = "Perfomance";
            // 
            // buckets3MaterialLabel
            // 
            this.buckets3MaterialLabel.AutoSize = true;
            this.buckets3MaterialLabel.Depth = 0;
            this.buckets3MaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buckets3MaterialLabel.Location = new System.Drawing.Point(539, 72);
            this.buckets3MaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.buckets3MaterialLabel.Name = "buckets3MaterialLabel";
            this.buckets3MaterialLabel.Size = new System.Drawing.Size(107, 19);
            this.buckets3MaterialLabel.TabIndex = 32;
            this.buckets3MaterialLabel.Text = "materialLabel5";
            // 
            // bucketsMaterialLabel
            // 
            this.bucketsMaterialLabel.AutoSize = true;
            this.bucketsMaterialLabel.Depth = 0;
            this.bucketsMaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bucketsMaterialLabel.Location = new System.Drawing.Point(539, 16);
            this.bucketsMaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.bucketsMaterialLabel.Name = "bucketsMaterialLabel";
            this.bucketsMaterialLabel.Size = new System.Drawing.Size(107, 19);
            this.bucketsMaterialLabel.TabIndex = 31;
            this.bucketsMaterialLabel.Text = "materialLabel4";
            // 
            // rmulti2MaterialLabel
            // 
            this.rmulti2MaterialLabel.AutoSize = true;
            this.rmulti2MaterialLabel.Depth = 0;
            this.rmulti2MaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rmulti2MaterialLabel.Location = new System.Drawing.Point(102, 72);
            this.rmulti2MaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.rmulti2MaterialLabel.Name = "rmulti2MaterialLabel";
            this.rmulti2MaterialLabel.Size = new System.Drawing.Size(107, 19);
            this.rmulti2MaterialLabel.TabIndex = 30;
            this.rmulti2MaterialLabel.Text = "materialLabel3";
            // 
            // threadsMaterialLabel
            // 
            this.threadsMaterialLabel.AutoSize = true;
            this.threadsMaterialLabel.Depth = 0;
            this.threadsMaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.threadsMaterialLabel.Location = new System.Drawing.Point(102, 16);
            this.threadsMaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.threadsMaterialLabel.Name = "threadsMaterialLabel";
            this.threadsMaterialLabel.Size = new System.Drawing.Size(107, 19);
            this.threadsMaterialLabel.TabIndex = 29;
            this.threadsMaterialLabel.Text = "materialLabel2";
            // 
            // rmulti2MaterialNumericUpDown
            // 
            this.rmulti2MaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.rmulti2MaterialNumericUpDown.Location = new System.Drawing.Point(3, 60);
            this.rmulti2MaterialNumericUpDown.LogValue = false;
            this.rmulti2MaterialNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.rmulti2MaterialNumericUpDown.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.rmulti2MaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.rmulti2MaterialNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rmulti2MaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.rmulti2MaterialNumericUpDown.Name = "rmulti2MaterialNumericUpDown";
            this.rmulti2MaterialNumericUpDown.Size = new System.Drawing.Size(96, 48);
            this.rmulti2MaterialNumericUpDown.TabIndex = 28;
            this.rmulti2MaterialNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // threadsMaterialNumericUpDown
            // 
            this.threadsMaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.threadsMaterialNumericUpDown.Location = new System.Drawing.Point(3, 6);
            this.threadsMaterialNumericUpDown.LogValue = false;
            this.threadsMaterialNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.threadsMaterialNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.threadsMaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.threadsMaterialNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadsMaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.threadsMaterialNumericUpDown.Name = "threadsMaterialNumericUpDown";
            this.threadsMaterialNumericUpDown.Size = new System.Drawing.Size(96, 48);
            this.threadsMaterialNumericUpDown.TabIndex = 27;
            this.threadsMaterialNumericUpDown.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // buckets3MaterialNumericUpDown
            // 
            this.buckets3MaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.buckets3MaterialNumericUpDown.Location = new System.Drawing.Point(435, 60);
            this.buckets3MaterialNumericUpDown.LogValue = true;
            this.buckets3MaterialNumericUpDown.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.buckets3MaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.buckets3MaterialNumericUpDown.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.buckets3MaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.buckets3MaterialNumericUpDown.Name = "buckets3MaterialNumericUpDown";
            this.buckets3MaterialNumericUpDown.Size = new System.Drawing.Size(98, 48);
            this.buckets3MaterialNumericUpDown.TabIndex = 1;
            this.buckets3MaterialNumericUpDown.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // bucketsMaterialNumericUpDown
            // 
            this.bucketsMaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.bucketsMaterialNumericUpDown.Location = new System.Drawing.Point(435, 6);
            this.bucketsMaterialNumericUpDown.LogValue = true;
            this.bucketsMaterialNumericUpDown.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.bucketsMaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.bucketsMaterialNumericUpDown.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.bucketsMaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.bucketsMaterialNumericUpDown.Name = "bucketsMaterialNumericUpDown";
            this.bucketsMaterialNumericUpDown.Size = new System.Drawing.Size(98, 48);
            this.bucketsMaterialNumericUpDown.TabIndex = 0;
            this.bucketsMaterialNumericUpDown.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // directoriesTabPage
            // 
            this.directoriesTabPage.Controls.Add(this.tmpdirMaterialTextBox2);
            this.directoriesTabPage.Controls.Add(this.tmpdir2MaterialTextBox2);
            this.directoriesTabPage.Controls.Add(this.stagedirMaterialTextBox2);
            this.directoriesTabPage.Controls.Add(this.directoutMaterialSwitch);
            this.directoriesTabPage.Controls.Add(this.stagedirMaterialButton);
            this.directoriesTabPage.Controls.Add(this.tmptoggleMaterialSwitch);
            this.directoriesTabPage.Controls.Add(this.tmpdirMaterialButton);
            this.directoriesTabPage.Controls.Add(this.waitforcopyMaterialSwitch);
            this.directoriesTabPage.Controls.Add(this.tmpdir2MaterialButton);
            this.directoriesTabPage.Location = new System.Drawing.Point(4, 22);
            this.directoriesTabPage.Name = "directoriesTabPage";
            this.directoriesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.directoriesTabPage.Size = new System.Drawing.Size(969, 224);
            this.directoriesTabPage.TabIndex = 1;
            this.directoriesTabPage.Text = "Directories";
            // 
            // tmpdirMaterialTextBox2
            // 
            this.tmpdirMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tmpdirMaterialTextBox2.AnimateReadOnly = false;
            this.tmpdirMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tmpdirMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tmpdirMaterialTextBox2.Depth = 0;
            this.tmpdirMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tmpdirMaterialTextBox2.HideSelection = true;
            this.tmpdirMaterialTextBox2.LeadingIcon = null;
            this.tmpdirMaterialTextBox2.Location = new System.Drawing.Point(6, 6);
            this.tmpdirMaterialTextBox2.MaxLength = 32767;
            this.tmpdirMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.tmpdirMaterialTextBox2.Name = "tmpdirMaterialTextBox2";
            this.tmpdirMaterialTextBox2.PasswordChar = '\0';
            this.tmpdirMaterialTextBox2.PrefixSuffixText = null;
            this.tmpdirMaterialTextBox2.ReadOnly = false;
            this.tmpdirMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tmpdirMaterialTextBox2.SelectedText = "";
            this.tmpdirMaterialTextBox2.SelectionLength = 0;
            this.tmpdirMaterialTextBox2.SelectionStart = 0;
            this.tmpdirMaterialTextBox2.ShortcutsEnabled = true;
            this.tmpdirMaterialTextBox2.Size = new System.Drawing.Size(521, 48);
            this.tmpdirMaterialTextBox2.TabIndex = 3;
            this.tmpdirMaterialTextBox2.TabStop = false;
            this.tmpdirMaterialTextBox2.Tag = "tmpdir";
            this.tmpdirMaterialTextBox2.Text = "tmpdirMaterialTextBox2";
            this.tmpdirMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tmpdirMaterialTextBox2.TrailingIcon = null;
            this.tmpdirMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // tmpdir2MaterialTextBox2
            // 
            this.tmpdir2MaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tmpdir2MaterialTextBox2.AnimateReadOnly = false;
            this.tmpdir2MaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tmpdir2MaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tmpdir2MaterialTextBox2.Depth = 0;
            this.tmpdir2MaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tmpdir2MaterialTextBox2.HideSelection = true;
            this.tmpdir2MaterialTextBox2.LeadingIcon = null;
            this.tmpdir2MaterialTextBox2.Location = new System.Drawing.Point(6, 60);
            this.tmpdir2MaterialTextBox2.MaxLength = 32767;
            this.tmpdir2MaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.tmpdir2MaterialTextBox2.Name = "tmpdir2MaterialTextBox2";
            this.tmpdir2MaterialTextBox2.PasswordChar = '\0';
            this.tmpdir2MaterialTextBox2.PrefixSuffixText = null;
            this.tmpdir2MaterialTextBox2.ReadOnly = false;
            this.tmpdir2MaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tmpdir2MaterialTextBox2.SelectedText = "";
            this.tmpdir2MaterialTextBox2.SelectionLength = 0;
            this.tmpdir2MaterialTextBox2.SelectionStart = 0;
            this.tmpdir2MaterialTextBox2.ShortcutsEnabled = true;
            this.tmpdir2MaterialTextBox2.Size = new System.Drawing.Size(521, 48);
            this.tmpdir2MaterialTextBox2.TabIndex = 4;
            this.tmpdir2MaterialTextBox2.TabStop = false;
            this.tmpdir2MaterialTextBox2.Text = "tmpdir2MaterialTextBox2";
            this.tmpdir2MaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tmpdir2MaterialTextBox2.TrailingIcon = null;
            this.tmpdir2MaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // stagedirMaterialTextBox2
            // 
            this.stagedirMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stagedirMaterialTextBox2.AnimateReadOnly = false;
            this.stagedirMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.stagedirMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.stagedirMaterialTextBox2.Depth = 0;
            this.stagedirMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.stagedirMaterialTextBox2.HideSelection = true;
            this.stagedirMaterialTextBox2.LeadingIcon = null;
            this.stagedirMaterialTextBox2.Location = new System.Drawing.Point(6, 114);
            this.stagedirMaterialTextBox2.MaxLength = 32767;
            this.stagedirMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.stagedirMaterialTextBox2.Name = "stagedirMaterialTextBox2";
            this.stagedirMaterialTextBox2.PasswordChar = '\0';
            this.stagedirMaterialTextBox2.PrefixSuffixText = null;
            this.stagedirMaterialTextBox2.ReadOnly = false;
            this.stagedirMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stagedirMaterialTextBox2.SelectedText = "";
            this.stagedirMaterialTextBox2.SelectionLength = 0;
            this.stagedirMaterialTextBox2.SelectionStart = 0;
            this.stagedirMaterialTextBox2.ShortcutsEnabled = true;
            this.stagedirMaterialTextBox2.Size = new System.Drawing.Size(521, 48);
            this.stagedirMaterialTextBox2.TabIndex = 6;
            this.stagedirMaterialTextBox2.TabStop = false;
            this.stagedirMaterialTextBox2.Text = "stagedirMaterialTextBox2";
            this.stagedirMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stagedirMaterialTextBox2.TrailingIcon = null;
            this.stagedirMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // directoutMaterialSwitch
            // 
            this.directoutMaterialSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.directoutMaterialSwitch.Depth = 0;
            this.directoutMaterialSwitch.Location = new System.Drawing.Point(625, 104);
            this.directoutMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.directoutMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.directoutMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.directoutMaterialSwitch.Name = "directoutMaterialSwitch";
            this.directoutMaterialSwitch.Ripple = true;
            this.directoutMaterialSwitch.Size = new System.Drawing.Size(284, 37);
            this.directoutMaterialSwitch.TabIndex = 9;
            this.directoutMaterialSwitch.Text = "directoutMaterialSwitch";
            this.directoutMaterialSwitch.UseVisualStyleBackColor = true;
            // 
            // stagedirMaterialButton
            // 
            this.stagedirMaterialButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stagedirMaterialButton.AutoSize = false;
            this.stagedirMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stagedirMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.stagedirMaterialButton.Depth = 0;
            this.stagedirMaterialButton.HighEmphasis = true;
            this.stagedirMaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder;
            this.stagedirMaterialButton.Location = new System.Drawing.Point(530, 114);
            this.stagedirMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.stagedirMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.stagedirMaterialButton.Name = "stagedirMaterialButton";
            this.stagedirMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.stagedirMaterialButton.Size = new System.Drawing.Size(48, 48);
            this.stagedirMaterialButton.TabIndex = 21;
            this.stagedirMaterialButton.Tag = "tmpdir";
            this.stagedirMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.stagedirMaterialButton.UseAccentColor = false;
            this.stagedirMaterialButton.UseVisualStyleBackColor = true;
            this.stagedirMaterialButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // tmptoggleMaterialSwitch
            // 
            this.tmptoggleMaterialSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tmptoggleMaterialSwitch.Depth = 0;
            this.tmptoggleMaterialSwitch.Location = new System.Drawing.Point(625, 67);
            this.tmptoggleMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.tmptoggleMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.tmptoggleMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.tmptoggleMaterialSwitch.Name = "tmptoggleMaterialSwitch";
            this.tmptoggleMaterialSwitch.Ripple = true;
            this.tmptoggleMaterialSwitch.Size = new System.Drawing.Size(284, 37);
            this.tmptoggleMaterialSwitch.TabIndex = 8;
            this.tmptoggleMaterialSwitch.Text = "tmptoggleMaterialSwitch";
            this.tmptoggleMaterialSwitch.UseVisualStyleBackColor = true;
            // 
            // tmpdirMaterialButton
            // 
            this.tmpdirMaterialButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tmpdirMaterialButton.AutoSize = false;
            this.tmpdirMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tmpdirMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.tmpdirMaterialButton.Depth = 0;
            this.tmpdirMaterialButton.HighEmphasis = true;
            this.tmpdirMaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder;
            this.tmpdirMaterialButton.Location = new System.Drawing.Point(530, 6);
            this.tmpdirMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tmpdirMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.tmpdirMaterialButton.Name = "tmpdirMaterialButton";
            this.tmpdirMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.tmpdirMaterialButton.Size = new System.Drawing.Size(48, 48);
            this.tmpdirMaterialButton.TabIndex = 11;
            this.tmpdirMaterialButton.Tag = "tmpdir";
            this.tmpdirMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.tmpdirMaterialButton.UseAccentColor = false;
            this.tmpdirMaterialButton.UseVisualStyleBackColor = true;
            this.tmpdirMaterialButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // waitforcopyMaterialSwitch
            // 
            this.waitforcopyMaterialSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.waitforcopyMaterialSwitch.Depth = 0;
            this.waitforcopyMaterialSwitch.Location = new System.Drawing.Point(625, 30);
            this.waitforcopyMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.waitforcopyMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.waitforcopyMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.waitforcopyMaterialSwitch.Name = "waitforcopyMaterialSwitch";
            this.waitforcopyMaterialSwitch.Ripple = true;
            this.waitforcopyMaterialSwitch.Size = new System.Drawing.Size(284, 37);
            this.waitforcopyMaterialSwitch.TabIndex = 7;
            this.waitforcopyMaterialSwitch.Text = "waitforcopyMaterialSwitch";
            this.waitforcopyMaterialSwitch.UseVisualStyleBackColor = true;
            // 
            // tmpdir2MaterialButton
            // 
            this.tmpdir2MaterialButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tmpdir2MaterialButton.AutoSize = false;
            this.tmpdir2MaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tmpdir2MaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.tmpdir2MaterialButton.Depth = 0;
            this.tmpdir2MaterialButton.HighEmphasis = true;
            this.tmpdir2MaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder;
            this.tmpdir2MaterialButton.Location = new System.Drawing.Point(530, 60);
            this.tmpdir2MaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tmpdir2MaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.tmpdir2MaterialButton.Name = "tmpdir2MaterialButton";
            this.tmpdir2MaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.tmpdir2MaterialButton.Size = new System.Drawing.Size(48, 48);
            this.tmpdir2MaterialButton.TabIndex = 20;
            this.tmpdir2MaterialButton.Tag = "tmpdir";
            this.tmpdir2MaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.tmpdir2MaterialButton.UseAccentColor = false;
            this.tmpdir2MaterialButton.UseVisualStyleBackColor = true;
            this.tmpdir2MaterialButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // sizeMaterialNumericUpDown
            // 
            this.sizeMaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.sizeMaterialNumericUpDown.Location = new System.Drawing.Point(10, 70);
            this.sizeMaterialNumericUpDown.LogValue = false;
            this.sizeMaterialNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.sizeMaterialNumericUpDown.Maximum = new decimal(new int[] {
            34,
            0,
            0,
            0});
            this.sizeMaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.sizeMaterialNumericUpDown.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.sizeMaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.sizeMaterialNumericUpDown.Name = "sizeMaterialNumericUpDown";
            this.sizeMaterialNumericUpDown.Size = new System.Drawing.Size(96, 48);
            this.sizeMaterialNumericUpDown.TabIndex = 13;
            this.sizeMaterialNumericUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // countMaterialNumericUpDown
            // 
            this.countMaterialNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.countMaterialNumericUpDown.ForeColor = System.Drawing.SystemColors.Control;
            this.countMaterialNumericUpDown.Location = new System.Drawing.Point(704, 70);
            this.countMaterialNumericUpDown.LogValue = false;
            this.countMaterialNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.countMaterialNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.countMaterialNumericUpDown.MaximumSize = new System.Drawing.Size(999, 0);
            this.countMaterialNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.countMaterialNumericUpDown.MinimumSize = new System.Drawing.Size(48, 48);
            this.countMaterialNumericUpDown.Name = "countMaterialNumericUpDown";
            this.countMaterialNumericUpDown.Size = new System.Drawing.Size(96, 48);
            this.countMaterialNumericUpDown.TabIndex = 15;
            this.countMaterialNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(10, 133);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(977, 48);
            this.materialTabSelector1.TabIndex = 24;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // startMaterialButton
            // 
            this.startMaterialButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startMaterialButton.AutoSize = false;
            this.startMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.startMaterialButton.Depth = 0;
            this.startMaterialButton.HighEmphasis = true;
            this.startMaterialButton.Icon = null;
            this.startMaterialButton.Location = new System.Drawing.Point(829, 76);
            this.startMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.startMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.startMaterialButton.Name = "startMaterialButton";
            this.startMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.startMaterialButton.Size = new System.Drawing.Size(158, 36);
            this.startMaterialButton.TabIndex = 18;
            this.startMaterialButton.Text = "Start";
            this.startMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.startMaterialButton.UseAccentColor = false;
            this.startMaterialButton.UseVisualStyleBackColor = true;
            this.startMaterialButton.Click += new System.EventHandler(this.startMaterialButton_Click);
            // 
            // finaldirMaterialButton
            // 
            this.finaldirMaterialButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.finaldirMaterialButton.AutoSize = false;
            this.finaldirMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.finaldirMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.finaldirMaterialButton.Depth = 0;
            this.finaldirMaterialButton.HighEmphasis = true;
            this.finaldirMaterialButton.Icon = global::MMX_NODE_GUI.Properties.Resources.folder;
            this.finaldirMaterialButton.Location = new System.Drawing.Point(990, 10);
            this.finaldirMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.finaldirMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.finaldirMaterialButton.Name = "finaldirMaterialButton";
            this.finaldirMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.finaldirMaterialButton.Size = new System.Drawing.Size(48, 48);
            this.finaldirMaterialButton.TabIndex = 14;
            this.finaldirMaterialButton.Tag = "tmpdir";
            this.finaldirMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.finaldirMaterialButton.UseAccentColor = false;
            this.finaldirMaterialButton.UseVisualStyleBackColor = true;
            this.finaldirMaterialButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // sizeMaterialLabel
            // 
            this.sizeMaterialLabel.AutoSize = true;
            this.sizeMaterialLabel.Depth = 0;
            this.sizeMaterialLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.sizeMaterialLabel.Location = new System.Drawing.Point(114, 86);
            this.sizeMaterialLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.sizeMaterialLabel.Name = "sizeMaterialLabel";
            this.sizeMaterialLabel.Size = new System.Drawing.Size(43, 19);
            this.sizeMaterialLabel.TabIndex = 19;
            this.sizeMaterialLabel.Text = "K size";
            // 
            // finaldirMaterialTextBox2
            // 
            this.finaldirMaterialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finaldirMaterialTextBox2.AnimateReadOnly = false;
            this.finaldirMaterialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.finaldirMaterialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.finaldirMaterialTextBox2.Depth = 0;
            this.finaldirMaterialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.finaldirMaterialTextBox2.HideSelection = true;
            this.finaldirMaterialTextBox2.LeadingIcon = null;
            this.finaldirMaterialTextBox2.Location = new System.Drawing.Point(10, 10);
            this.finaldirMaterialTextBox2.MaxLength = 32767;
            this.finaldirMaterialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.finaldirMaterialTextBox2.Name = "finaldirMaterialTextBox2";
            this.finaldirMaterialTextBox2.PasswordChar = '\0';
            this.finaldirMaterialTextBox2.PrefixSuffixText = null;
            this.finaldirMaterialTextBox2.ReadOnly = false;
            this.finaldirMaterialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.finaldirMaterialTextBox2.SelectedText = "";
            this.finaldirMaterialTextBox2.SelectionLength = 0;
            this.finaldirMaterialTextBox2.SelectionStart = 0;
            this.finaldirMaterialTextBox2.ShortcutsEnabled = true;
            this.finaldirMaterialTextBox2.Size = new System.Drawing.Size(977, 48);
            this.finaldirMaterialTextBox2.TabIndex = 5;
            this.finaldirMaterialTextBox2.TabStop = false;
            this.finaldirMaterialTextBox2.Text = "finaldirMaterialTextBox2";
            this.finaldirMaterialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.finaldirMaterialTextBox2.TrailingIcon = null;
            this.finaldirMaterialTextBox2.UseSystemPasswordChar = false;
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.groupBox2);
            this.settingsTabPage.Controls.Add(this.confirmationOnExitMaterialSwitch);
            this.settingsTabPage.Controls.Add(this.startMinimizedMaterialSwitch);
            this.settingsTabPage.Controls.Add(this.startOnStartupMaterialSwitch);
            this.settingsTabPage.Controls.Add(this.showInNotifitationMaterialSwitch);
            this.settingsTabPage.Controls.Add(this.showInNotifitationGroupBox);
            this.settingsTabPage.ImageKey = "cog.png";
            this.settingsTabPage.Location = new System.Drawing.Point(4, 31);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.settingsTabPage.Size = new System.Drawing.Size(1056, 613);
            this.settingsTabPage.TabIndex = 2;
            this.settingsTabPage.Text = "GUI Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.inhibitSystemSleepMaterialSwitch);
            this.groupBox2.Location = new System.Drawing.Point(13, 262);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(30, 15, 10, 10);
            this.groupBox2.Size = new System.Drawing.Size(973, 76);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Power Management";
            // 
            // inhibitSystemSleepMaterialSwitch
            // 
            this.inhibitSystemSleepMaterialSwitch.AutoSize = true;
            this.inhibitSystemSleepMaterialSwitch.Depth = 0;
            this.inhibitSystemSleepMaterialSwitch.Location = new System.Drawing.Point(30, 29);
            this.inhibitSystemSleepMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.inhibitSystemSleepMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.inhibitSystemSleepMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.inhibitSystemSleepMaterialSwitch.Name = "inhibitSystemSleepMaterialSwitch";
            this.inhibitSystemSleepMaterialSwitch.Ripple = true;
            this.inhibitSystemSleepMaterialSwitch.Size = new System.Drawing.Size(198, 37);
            this.inhibitSystemSleepMaterialSwitch.TabIndex = 11;
            this.inhibitSystemSleepMaterialSwitch.Text = "Inhibit system sleep";
            this.inhibitSystemSleepMaterialSwitch.UseVisualStyleBackColor = true;
            this.inhibitSystemSleepMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // confirmationOnExitMaterialSwitch
            // 
            this.confirmationOnExitMaterialSwitch.AutoSize = true;
            this.confirmationOnExitMaterialSwitch.Depth = 0;
            this.confirmationOnExitMaterialSwitch.Location = new System.Drawing.Point(10, 81);
            this.confirmationOnExitMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.confirmationOnExitMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.confirmationOnExitMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.confirmationOnExitMaterialSwitch.Name = "confirmationOnExitMaterialSwitch";
            this.confirmationOnExitMaterialSwitch.Ripple = true;
            this.confirmationOnExitMaterialSwitch.Size = new System.Drawing.Size(202, 37);
            this.confirmationOnExitMaterialSwitch.TabIndex = 13;
            this.confirmationOnExitMaterialSwitch.Text = "Confirmation on exit";
            this.confirmationOnExitMaterialSwitch.UseVisualStyleBackColor = true;
            this.confirmationOnExitMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // startMinimizedMaterialSwitch
            // 
            this.startMinimizedMaterialSwitch.AutoSize = true;
            this.startMinimizedMaterialSwitch.Depth = 0;
            this.startMinimizedMaterialSwitch.Location = new System.Drawing.Point(10, 44);
            this.startMinimizedMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.startMinimizedMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.startMinimizedMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.startMinimizedMaterialSwitch.Name = "startMinimizedMaterialSwitch";
            this.startMinimizedMaterialSwitch.Ripple = true;
            this.startMinimizedMaterialSwitch.Size = new System.Drawing.Size(170, 37);
            this.startMinimizedMaterialSwitch.TabIndex = 12;
            this.startMinimizedMaterialSwitch.Text = "Start minimized";
            this.startMinimizedMaterialSwitch.UseVisualStyleBackColor = true;
            this.startMinimizedMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // startOnStartupMaterialSwitch
            // 
            this.startOnStartupMaterialSwitch.AutoSize = true;
            this.startOnStartupMaterialSwitch.Depth = 0;
            this.startOnStartupMaterialSwitch.Location = new System.Drawing.Point(10, 10);
            this.startOnStartupMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.startOnStartupMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.startOnStartupMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.startOnStartupMaterialSwitch.Name = "startOnStartupMaterialSwitch";
            this.startOnStartupMaterialSwitch.Ripple = true;
            this.startOnStartupMaterialSwitch.Size = new System.Drawing.Size(241, 37);
            this.startOnStartupMaterialSwitch.TabIndex = 11;
            this.startOnStartupMaterialSwitch.Text = "Start on Windows start up";
            this.startOnStartupMaterialSwitch.UseVisualStyleBackColor = true;
            this.startOnStartupMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // showInNotifitationMaterialSwitch
            // 
            this.showInNotifitationMaterialSwitch.AutoSize = true;
            this.showInNotifitationMaterialSwitch.Depth = 0;
            this.showInNotifitationMaterialSwitch.Location = new System.Drawing.Point(374, 84);
            this.showInNotifitationMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.showInNotifitationMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.showInNotifitationMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.showInNotifitationMaterialSwitch.Name = "showInNotifitationMaterialSwitch";
            this.showInNotifitationMaterialSwitch.Ripple = true;
            this.showInNotifitationMaterialSwitch.Size = new System.Drawing.Size(243, 37);
            this.showInNotifitationMaterialSwitch.TabIndex = 16;
            this.showInNotifitationMaterialSwitch.Text = "Show in notification area  ";
            this.showInNotifitationMaterialSwitch.UseVisualStyleBackColor = true;
            this.showInNotifitationMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // showInNotifitationGroupBox
            // 
            this.showInNotifitationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showInNotifitationGroupBox.Controls.Add(this.closeToNotificationMaterialSwitch);
            this.showInNotifitationGroupBox.Controls.Add(this.minimizeToNotificationMaterialSwitch);
            this.showInNotifitationGroupBox.Location = new System.Drawing.Point(13, 137);
            this.showInNotifitationGroupBox.Name = "showInNotifitationGroupBox";
            this.showInNotifitationGroupBox.Padding = new System.Windows.Forms.Padding(30, 15, 10, 10);
            this.showInNotifitationGroupBox.Size = new System.Drawing.Size(973, 119);
            this.showInNotifitationGroupBox.TabIndex = 14;
            this.showInNotifitationGroupBox.TabStop = false;
            // 
            // closeToNotificationMaterialSwitch
            // 
            this.closeToNotificationMaterialSwitch.AutoSize = true;
            this.closeToNotificationMaterialSwitch.Depth = 0;
            this.closeToNotificationMaterialSwitch.Location = new System.Drawing.Point(30, 65);
            this.closeToNotificationMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.closeToNotificationMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.closeToNotificationMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.closeToNotificationMaterialSwitch.Name = "closeToNotificationMaterialSwitch";
            this.closeToNotificationMaterialSwitch.Ripple = true;
            this.closeToNotificationMaterialSwitch.Size = new System.Drawing.Size(235, 37);
            this.closeToNotificationMaterialSwitch.TabIndex = 7;
            this.closeToNotificationMaterialSwitch.Text = "Close to notification area";
            this.closeToNotificationMaterialSwitch.UseVisualStyleBackColor = true;
            this.closeToNotificationMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // minimizeToNotificationMaterialSwitch
            // 
            this.minimizeToNotificationMaterialSwitch.AutoSize = true;
            this.minimizeToNotificationMaterialSwitch.Depth = 0;
            this.minimizeToNotificationMaterialSwitch.Location = new System.Drawing.Point(30, 28);
            this.minimizeToNotificationMaterialSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.minimizeToNotificationMaterialSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.minimizeToNotificationMaterialSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.minimizeToNotificationMaterialSwitch.Name = "minimizeToNotificationMaterialSwitch";
            this.minimizeToNotificationMaterialSwitch.Ripple = true;
            this.minimizeToNotificationMaterialSwitch.Size = new System.Drawing.Size(261, 37);
            this.minimizeToNotificationMaterialSwitch.TabIndex = 6;
            this.minimizeToNotificationMaterialSwitch.Text = "Minimize to notification area";
            this.minimizeToNotificationMaterialSwitch.UseVisualStyleBackColor = true;
            this.minimizeToNotificationMaterialSwitch.CheckStateChanged += new System.EventHandler(this.saveSettingsMaterialSwitch_CheckStateChanged);
            // 
            // menuIconList
            // 
            this.menuIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("menuIconList.ImageStream")));
            this.menuIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.menuIconList.Images.SetKeyName(0, "cog.png");
            this.menuIconList.Images.SetKeyName(1, "database-sync.png");
            this.menuIconList.Images.SetKeyName(2, "tractor.png");
            this.menuIconList.Images.SetKeyName(3, "harddisk-plus.png");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(3, 715);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1064, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 740);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MenuMaterialTabControl);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.MenuMaterialTabControl;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1070, 678);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MMX Node";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.MenuMaterialTabControl.ResumeLayout(false);
            this.harvesterTabPage.ResumeLayout(false);
            this.harvesterTabPage.PerformLayout();
            this.plotterTabPage.ResumeLayout(false);
            this.plotterTabPage.PerformLayout();
            this.materialTabControl1.ResumeLayout(false);
            this.keysTabPage.ResumeLayout(false);
            this.keysTabPage.PerformLayout();
            this.perfomanceTabPage.ResumeLayout(false);
            this.perfomanceTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rmulti2MaterialNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsMaterialNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buckets3MaterialNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bucketsMaterialNumericUpDown)).EndInit();
            this.directoriesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sizeMaterialNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countMaterialNumericUpDown)).EndInit();
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabPage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.showInNotifitationGroupBox.ResumeLayout(false);
            this.showInNotifitationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.TabPage nodeTabPage;
        private MaterialSkin.Controls.MaterialTabControl MenuMaterialTabControl;
        private System.Windows.Forms.ImageList menuIconList;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private MaterialSkin.Controls.MaterialSwitch inhibitSystemSleepMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch confirmationOnExitMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch startMinimizedMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch startOnStartupMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch showInNotifitationMaterialSwitch;
        private System.Windows.Forms.GroupBox showInNotifitationGroupBox;
        private MaterialSkin.Controls.MaterialSwitch closeToNotificationMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch minimizeToNotificationMaterialSwitch;
        private System.Windows.Forms.TabPage harvesterTabPage;
        private MaterialSkin.Controls.MaterialListBox plotFoldersMaterialListBox;
        private MaterialSkin.Controls.MaterialButton removePlotFolderMaterialButton;
        private MaterialSkin.Controls.MaterialButton addPlotFolderMaterialButton;
        private MaterialSkin.Controls.MaterialTextBox2 finaldirMaterialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox2 tmpdir2MaterialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox2 tmpdirMaterialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox2 stagedirMaterialTextBox2;
        private MaterialSkin.Controls.MaterialSwitch directoutMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch tmptoggleMaterialSwitch;
        private MaterialSkin.Controls.MaterialSwitch waitforcopyMaterialSwitch;
        private MaterialSkin.Controls.MaterialButton tmpdirMaterialButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private MaterialNumericUpDown sizeMaterialNumericUpDown;
        private MaterialSkin.Controls.MaterialButton finaldirMaterialButton;
        private MaterialNumericUpDown countMaterialNumericUpDown;
        private MaterialSkin.Controls.MaterialLabel countMaterialLabel;
        private MaterialSkin.Controls.MaterialButton startMaterialButton;
        private MaterialSkin.Controls.MaterialLabel sizeMaterialLabel;
        private MaterialSkin.Controls.MaterialButton stagedirMaterialButton;
        private MaterialSkin.Controls.MaterialButton tmpdir2MaterialButton;
        private System.Windows.Forms.TabPage plotterTabPage;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage keysTabPage;
        private MaterialSkin.Controls.MaterialTextBox2 farmerkeyMaterialTextBox2;
        private MaterialSkin.Controls.MaterialSwitch nftplotMaterialSwitch;
        private MaterialSkin.Controls.MaterialTextBox2 contractMaterialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox2 poolkeyMaterialTextBox2;
        private System.Windows.Forms.TabPage directoriesTabPage;
        private System.Windows.Forms.TabPage perfomanceTabPage;
        private MaterialSkin.Controls.MaterialMultiLineTextBox2 materialMultiLineTextBox21;
        private MaterialSkin.Controls.MaterialLabel harvesterPlotsMaterialLabel;
        private MaterialNumericUpDown bucketsMaterialNumericUpDown;
        private MaterialNumericUpDown threadsMaterialNumericUpDown;
        private MaterialNumericUpDown buckets3MaterialNumericUpDown;
        private MaterialNumericUpDown rmulti2MaterialNumericUpDown;
        private MaterialSkin.Controls.MaterialLabel buckets3MaterialLabel;
        private MaterialSkin.Controls.MaterialLabel bucketsMaterialLabel;
        private MaterialSkin.Controls.MaterialLabel rmulti2MaterialLabel;
        private MaterialSkin.Controls.MaterialLabel threadsMaterialLabel;
    }
}

