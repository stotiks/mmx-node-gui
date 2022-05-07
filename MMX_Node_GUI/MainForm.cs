using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{

    public partial class MainForm : Form
    {
        private const string gitHubUrl = "https://github.com/madMAx43v3r/mmx-node";
        private const string wikiUrl = "https://github.com/madMAx43v3r/mmx-node/wiki";
        private const string discordUrl = "https://discord.gg/tCwevssVmY";
        private const string explorerUrl = "http://94.130.47.147/recent";

        private readonly Node node;
        private bool disableCloseToNotification = false;

        public bool CloseToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.closeToNotification && !disableCloseToNotification;

        public bool MinimizeToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.minimizeToNotification;

        public MainForm()
        {
            node = new Node();

            node.Started += new EventHandler(refreshToolStripMenuItem_Click);
            node.BeforeStop += new EventHandler((object sender, EventArgs e) => CefSharp.WebBrowserExtensions.LoadHtml(chromiumWebBrowser1, GetLoadingHtml(), Node.baseUri.ToString()));

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            notifyIcon1.Visible = Properties.Settings.Default.showInNotifitation;

            if (Properties.Settings.Default.startMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }

            chromiumWebBrowser1.FrameLoadEnd += StartNode;
            CefSharp.WebBrowserExtensions.LoadHtml(chromiumWebBrowser1, GetLoadingHtml(), Node.baseUri.ToString());
        }

        private void StartNode(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                chromiumWebBrowser1.FrameLoadEnd -= StartNode;
                node.Start();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (MinimizeToNotification && this.WindowState == FormWindowState.Minimized)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Hide();
                }));
                //notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            //notifyIcon1.Visible = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseToNotification)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                return;
            }

            if (Properties.Settings.Default.confirmationOnExit && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to close the application",
                                                            System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            node.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableCloseToNotification = true;
            Close();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.LoadUrl(Node.guiUri.ToString());
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

        private string GetLoadingHtml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("loading.html"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();

            notifyIcon1.Visible = Properties.Settings.Default.showInNotifitation;
        }

    }


}
