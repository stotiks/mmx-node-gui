using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{

    public partial class MainForm : MaterialForm
    {
        private const string gitHubUrl = "https://github.com/madMAx43v3r/mmx-node";
        private const string wikiUrl = "https://github.com/madMAx43v3r/mmx-node/wiki";
        private const string discordUrl = "https://discord.gg/tCwevssVmY";
        private const string explorerUrl = "http://94.130.47.147/recent";


        private bool disableCloseToNotification = false;

        public bool CloseToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.closeToNotification && !disableCloseToNotification;

        public bool MinimizeToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.minimizeToNotification;

        public MainForm()
        {
            InitializeSkinManager();

            InitializeComponent();

            InitializeNode();

            InitializeSettings();

            InitializeHarvester();

            InitializePlotter();

            InitializeLoc();

        }

        private void InitializeSkinManager()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Blue200, TextShade.WHITE);
            //this.FormStyle = FormStyles.ActionBar_None;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            notifyIcon1.Visible = Properties.Settings.Default.showInNotifitation;

            if (Properties.Settings.Default.startMinimized)
            {
                WindowState = FormWindowState.Minimized;
            }

            MainForm_Node_Load();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (MinimizeToNotification && this.WindowState == FormWindowState.Minimized)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Hide();
                }));
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        bool closePending = false;
        private CultureInfo culture;

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closePending) return;

            if (CloseToNotification)
            {
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                return;
            }

            if (Properties.Settings.Default.confirmationOnExit && e.CloseReason == CloseReason.UserClosing)
            {
                MaterialDialog materialDialog = new MaterialDialog(this,
                                                                   Assembly.GetExecutingAssembly().GetName().Name,
                                                                   "Do you want to close the application?",
                                                                   "No", true, "Yes");
                DialogResult dialogResult = materialDialog.ShowDialog(this);

                if (dialogResult == DialogResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }

            closePending = true;

            MainForm_Node_FormClosing();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableCloseToNotification = true;
            Close();
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(gitHubUrl);
        }

        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(wikiUrl);
        }

        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(discordUrl);
        }

        private void explorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(explorerUrl);
        }


        // --------------------------------------------------------------------------------
        private Dictionary<string, string> launguages = new Dictionary<string, string>(){
            {"en", "English"},
            {"ru", "Русский"}
        };


        private void InitializeLoc()
        {
            langMaterialComboBox.DisplayMember = "Value";
            langMaterialComboBox.ValueMember = "Key";
            langMaterialComboBox.DataSource = new BindingSource(launguages, null);

            this.Culture = new CultureInfo(Properties.Settings.Default.langCode);
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
                        if (!entry.Value.GetType().Equals(typeof(string))) return;

                        string Key = entry.Key.ToString(),
                               Value = (string)entry.Value;

                        try
                        {
                            Control c = Controls.Find(Key, true).SingleOrDefault();
                            if (c != null && !(c is MaterialTextBox2))
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
                        if (!entry.Value.GetType().Equals(typeof(string))) return;

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
                    //this.OnCultureChanged();
                }
            }
        }

        private void langMaterialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lang = langMaterialComboBox.SelectedValue.ToString();
            this.Culture = new CultureInfo(lang);
            saveSettings(sender, e);
        }
    }
}
