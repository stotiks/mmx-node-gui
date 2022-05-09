using System;
using System.Drawing;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MaterialNumericUpDown : NumericUpDown
    {
 
        public MaterialNumericUpDown()
        {
            Controls[0].Visible = false;
            Controls[1].Visible = false;
            //Margin = new Padding(3, 3, 3, 3);
            InitializeComponent();

            upMaterialButton.Click += (o, e) => UpButton();
            downMaterialButton.Click += (o, e) => DownButton();
            materialTextBox2.KeyPress += (o, e) => OnTextBoxKeyPress(o, e);

            materialTextBox2.DataBindings.Add("Text", this, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            
        }

/*
                private void OnTextBoxKeyPress(object o, KeyPressEventArgs e)
                {
                    throw new NotImplementedException();
                }

                private void DownButton()
                {
                    throw new NotImplementedException();
                }

                private void UpButton()
                {
                    throw new NotImplementedException();
                }

                public decimal Maximum { get; internal set; }
                public decimal Minimum { get; internal set; }
                public decimal Value { get; internal set; }
*/
  


        protected override void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 48 || e.KeyChar > 57)
            {
                e.Handled = true;
            }
            else
            {
                base.OnTextBoxKeyPress(sender, e);
            }
        }
        /**/

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.Transparent);
            InvokePaintBackground(this, e);
        }

        public override Size MinimumSize
        {
            get
            {
                return new Size(48, 48);
            }
            set
            {
                base.MinimumSize = value;
            }
        }

    }
}
