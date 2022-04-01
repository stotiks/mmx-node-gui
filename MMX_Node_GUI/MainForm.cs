using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_GUI
{

    public partial class MainForm : Form
    {
        private const String url = "http://127.0.0.1:11380/gui/";

        private Process process;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartNode();
            chromiumWebBrowser1.LoadUrl(url);
        }

        private void StartNode()
        {
            process = new Process();
            process.StartInfo.WorkingDirectory = "C:\\Program Files\\MMX\\";
            process.StartInfo.FileName = "C:\\Program Files\\MMX\\run_node.cmd";
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            //process.OutputDataReceived += (sender1, args) => WriteTextSafe(args.Data);
            process.Exited += new EventHandler(nodeProcess_HasExited);

            process.OutputDataReceived += (sender1, args) => WriteProcessLog(args.Data);

            if (false)
            { 
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
            }

            WriteProcessLog(String.Format("{0} {1}\r\n", process.StartInfo.FileName, process.StartInfo.Arguments));
            process.Start();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
        }

        private void WriteProcessLog(string text)
        {
            Console.WriteLine(text);
        }

        private void nodeProcess_HasExited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to close the application", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) e.Cancel = true;

            if (ProcessExtensions.IsRunning(process))
            {
                process.CloseMainWindow();
            }
        }
    }


    public static class ProcessExtensions
    {
        public static bool IsRunning(this Process process)
        {
            if (process == null)
            {
                return false;
            }

            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
