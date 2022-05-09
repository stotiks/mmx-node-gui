namespace MMX_NODE_GUI
{
    partial class MaterialNumericUpDown
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialTextBox2 = new MaterialSkin.Controls.MaterialTextBox2();
            this.downMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.upMaterialButton = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // materialTextBox2
            // 
            this.materialTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTextBox2.AnimateReadOnly = false;
            this.materialTextBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBox2.Depth = 0;
            this.materialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTextBox2.HideSelection = true;
            this.materialTextBox2.LeadingIcon = null;
            this.materialTextBox2.Location = new System.Drawing.Point(0, 0);
            this.materialTextBox2.MaxLength = 32767;
            this.materialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.materialTextBox2.Name = "materialTextBox2";
            this.materialTextBox2.PasswordChar = '\0';
            this.materialTextBox2.PrefixSuffixText = null;
            this.materialTextBox2.ReadOnly = false;
            this.materialTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBox2.SelectedText = "";
            this.materialTextBox2.SelectionLength = 0;
            this.materialTextBox2.SelectionStart = 0;
            this.materialTextBox2.ShortcutsEnabled = true;
            this.materialTextBox2.Size = new System.Drawing.Size(172, 48);
            this.materialTextBox2.TabIndex = 0;
            this.materialTextBox2.TabStop = false;
            this.materialTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBox2.TrailingIcon = null;
            this.materialTextBox2.UseSystemPasswordChar = false;
            // 
            // downMaterialButton
            // 
            this.downMaterialButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.downMaterialButton.AutoSize = false;
            this.downMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.downMaterialButton.Depth = 0;
            this.downMaterialButton.HighEmphasis = true;
            this.downMaterialButton.Icon = null;
            this.downMaterialButton.Location = new System.Drawing.Point(174, 24);
            this.downMaterialButton.Margin = new System.Windows.Forms.Padding(1);
            this.downMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.downMaterialButton.Name = "downMaterialButton";
            this.downMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.downMaterialButton.Size = new System.Drawing.Size(23, 23);
            this.downMaterialButton.TabIndex = 2;
            this.downMaterialButton.Text = "🔽";
            this.downMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.downMaterialButton.UseAccentColor = false;
            this.downMaterialButton.UseVisualStyleBackColor = true;
            // 
            // upMaterialButton
            // 
            this.upMaterialButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.upMaterialButton.AutoSize = false;
            this.upMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upMaterialButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.upMaterialButton.Depth = 0;
            this.upMaterialButton.HighEmphasis = true;
            this.upMaterialButton.Icon = null;
            this.upMaterialButton.Location = new System.Drawing.Point(174, 0);
            this.upMaterialButton.Margin = new System.Windows.Forms.Padding(1);
            this.upMaterialButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.upMaterialButton.Name = "upMaterialButton";
            this.upMaterialButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.upMaterialButton.Size = new System.Drawing.Size(23, 23);
            this.upMaterialButton.TabIndex = 1;
            this.upMaterialButton.Text = "🔼";
            this.upMaterialButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.upMaterialButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.upMaterialButton.UseAccentColor = false;
            this.upMaterialButton.UseVisualStyleBackColor = true;
            // 
            // MaterialNumericUpDown
            // 
            this.Controls.Add(this.downMaterialButton);
            this.Controls.Add(this.upMaterialButton);
            this.Controls.Add(this.materialTextBox2);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.MaximumSize = new System.Drawing.Size(999, 0);
            this.MinimumSize = new System.Drawing.Size(86, 0);
            this.Name = "MaterialNumericUpDown";
            this.Size = new System.Drawing.Size(200, 48);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox2 materialTextBox2;
        private MaterialSkin.Controls.MaterialButton upMaterialButton;
        private MaterialSkin.Controls.MaterialButton downMaterialButton;
    }
}
