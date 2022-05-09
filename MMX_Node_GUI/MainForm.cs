using CefSharp;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

            NodeMainForm_Load();
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
            chromiumWebBrowser.LoadHtml(loadingHtml, Node.baseUri.ToString());
            nodeTabPage.Show();
            node.Stop();

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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

    }


}
