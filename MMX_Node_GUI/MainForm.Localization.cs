using MaterialSkin.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private CultureInfo culture;

        private Dictionary<string, string> launguages = new Dictionary<string, string>(){
            { "en", "English" },
            //{ "es", "Español" },
            { "de", "Deutsch" },
            { "nl", "Nederlands"},
            { "pt", "Português" },
            { "ru", "Русский" },
            { "uk", "Українська" },
        };


        private void InitializeLocalization()
        {
            langMaterialComboBox.DisplayMember = "Value";
            langMaterialComboBox.ValueMember = "Key";
            langMaterialComboBox.DataSource = new BindingSource(launguages, null);

            this.Culture = new CultureInfo(Properties.Settings.Default.langCode);
            Properties.Resources.Culture = this.Culture;

            CultureChanged += (s, e) => Properties.Resources.Culture = this.Culture;
        }


        /// <summary>
        /// Current culture of this form
        /// </summary>
        [Browsable(false)]
        [Description("Current culture of this form")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CultureInfo Culture
        {
            get { return this.culture; }
            set
            {
                if (this.culture != value)
                {
                    ResourceSet resourceSet = new ComponentResourceManager(GetType()).GetResourceSet(value, true, true);
                    IEnumerable<DictionaryEntry> entries = resourceSet
                        .Cast<DictionaryEntry>()
                        .Where(x => x.Key.ToString().Contains(".Text"))
                        .Select(x => { x.Key = x.Key.ToString().Replace(">", "").Split('.')[0]; return x; });

                    foreach (DictionaryEntry entry in entries)
                    {
                        if (!entry.Value.GetType().Equals(typeof(string))) continue;

                        string Key = entry.Key.ToString(),
                               Value = (string)entry.Value;

                        try
                        {
                            Control c = this.Controls.Find(Key, true).SingleOrDefault();
                            if (c != null && !(c is MaterialTextBox2 || c is MaterialMultiLineTextBox2))
                            {
                                c.Text = Value;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Control {0} is null in form {1}!", Key, GetType().Name);
                        }
                    }

                    IEnumerable<DictionaryEntry> entries2 = resourceSet
                        .Cast<DictionaryEntry>()
                        .Where(x => x.Key.ToString().Contains(".Hint"))
                        .Select(x => { x.Key = x.Key.ToString().Replace(">", "").Split('.')[0]; return x; });

                    foreach (DictionaryEntry entry in entries2)
                    {
                        if (!entry.Value.GetType().Equals(typeof(string))) continue;

                        string Key = entry.Key.ToString(),
                               Value = (string)entry.Value;

                        try
                        {
                            Control c = Controls.Find(Key, true).SingleOrDefault();
                            if (c is MaterialTextBox2)
                            {
                                (c as MaterialTextBox2).Hint = Value;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Control {0} is null in form {1}!", Key, GetType().Name);
                        }
                    }

                    this.culture = value;
                    this.OnCultureChanged();
                }
            }
        }

        public event EventHandler CultureChanged;

        private void OnCultureChanged()
        {
            if (CultureChanged != null)
            {
                CultureChanged(this, EventArgs.Empty);
            }
        }

    }
}
