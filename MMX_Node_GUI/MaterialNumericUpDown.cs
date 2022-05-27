using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MaterialNumericUpDown : NumericUpDown
    {
        private bool _log = false;
        public bool LogValue
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
            }
        }

        public MaterialNumericUpDown()
        {
            Controls[0].Visible = false;
            Controls[1].Visible = false;
            //Margin = new Padding(3, 3, 3, 3);
            InitializeComponent();

            upMaterialButton.Click += (o, e) => UpButton();
            downMaterialButton.Click += (o, e) => DownButton();
            materialTextBox2.KeyPress += (o, e) => OnTextBoxKeyPress(o, e);
            materialTextBox2.Leave += (o, e) =>
            {
                ValidateEditText();

                if(string.IsNullOrEmpty(Text))
                {
                    Value = Minimum;
                }

                materialTextBox2.Text = Value.ToString();

            };

            materialTextBox2.DataBindings.Add("Text", this, "Text", true, DataSourceUpdateMode.OnPropertyChanged);

        }
        public override void UpButton()
        {
            if (_log)
            {
                var pow = Math.Log((double)Value,2);
                //pow = Math.Ceiling(pow);
                pow++;

                var num = (decimal)Math.Pow(2, pow);

                if (num > Maximum)
                {
                    num = Maximum;
                }
                
                Value = num;
            }
            else
            {
                base.UpButton();
            }
        }

        public override void DownButton()
        {
            if (_log)
            {
                if (base.UserEdit)
                {
                    ParseEditTextLog();
                }

                var pow = Math.Log((double)Value, 2);
                //pow = Math.Ceiling(pow);
                pow--;

                var num = (decimal)Math.Pow(2, pow);

                if (num < Minimum)
                {
                    num = Minimum;
                }

                Value = num;
            }
            else
            {
                base.DownButton();
            }
        }

        protected override void ValidateEditText()
        {
            if (_log)
            {
                ParseEditTextLog();
                UpdateEditText();
            } else
            {
                base.ValidateEditText();
            }
        }

        protected void ParseEditTextLog()
        {
            try
            {
                if (!string.IsNullOrEmpty(Text) && (Text.Length != 1 || !(Text == "-")))
                {
                    var _value = ConstrainLog(decimal.Parse(Text, CultureInfo.CurrentCulture));
                    var pow = Math.Log((double)_value, 2);

                    if ( pow == Math.Ceiling(pow) )
                    {                        
                        Value = _value;
                    } else
                    {
                        Value = (decimal)Math.Pow(2, Math.Ceiling(pow));
                    }
                }
            }
            catch
            {
            }
            finally
            {
                base.UserEdit = false;
            }
        }

        private decimal ConstrainLog(decimal value)
        {
            if (value < Minimum)
            {
                value = Minimum;
            }

            if (value > Maximum)
            {
                value = Maximum;
            }

            return value;
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
            if ( (e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != Convert.ToChar(8) )
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
