using MMX_NODE_GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_GUI
{

    public partial class MainForm : Form
    {
        
        static private Uri baseUri = new Uri("http://127.0.0.1:11380");
        static private Uri guiUri = new Uri(baseUri, "/gui/");
        static private Uri exitUri = new Uri(baseUri, "/wapi/node/exit");
        static private Uri checkUri = new Uri(baseUri, "/api/router/get_peer_info");
        
        private ConsoleForm consoleForm;
        private ConsoleControl.ConsoleControl consoleControl;
        public event EventHandler NodeStartEvent;

        private static readonly HttpClient client = new HttpClient();

        public MainForm()
        {
            InitializeComponent();

            consoleForm = new ConsoleForm();
            consoleControl = consoleForm.consoleControl1;
            consoleControl.InternalRichTextBox.HideSelection = false;

            NodeStartEvent += new EventHandler(refreshToolStripMenuItem_Click);

            CefSharp.WebBrowserExtensions.LoadHtml(chromiumWebBrowser1, GetLoadingHtml(), baseUri.ToString());
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

        private void Form1_Load(object sender, EventArgs e)
        {
            StartNode();   
        }

        private void StartNode()
        {            
            var exePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
#if DEBUG
            exePath = "C:\\Program Files\\MMX";
#endif
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = exePath;
            psi.FileName = exePath + "\\run_node.cmd";
            consoleControl.StartProcess(psi);

            Task.Run(async () => {
                while (true)
                {
                    try
                    {
                        var result = await client.GetAsync(checkUri);
                        //Console.WriteLine(result);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                    Console.WriteLine("Waiting node...");
                    await Task.Delay(500);
                }

                OnNodeStart();
            });
        }  

        private void OnNodeStart()
        {
            if (NodeStartEvent != null)
            {
                NodeStartEvent(this, EventArgs.Empty);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
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
            DialogResult dialogResult = MessageBox.Show("Do you want to close the application", 
                                                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            } else
            {
                //chromiumWebBrowser1.LoadUrl("about:blank");
                CefSharp.WebBrowserExtensions.LoadHtml(chromiumWebBrowser1, GetLoadingHtml(), baseUri.ToString());
                Task.Run(async () => await ExitNodeAsync()).Wait();
                consoleControl.StopProcess();
            }

        }

        private async Task ExitNodeAsync() 
        {
            try
            {
                var result = await client.PostAsync(exitUri, null);
                //Console.WriteLine(result);
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleForm.Show();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.LoadUrl(guiUri.ToString());
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/madMAx43v3r/mmx-node");
        }

        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/madMAx43v3r/mmx-node/wiki");
        }

        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/tCwevssVmY");
        }

        private void explorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://94.130.47.147/recent");        
        }
    }


}
