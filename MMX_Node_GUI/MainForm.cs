﻿using System;
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
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = "C:\\Program Files\\MMX\\";
            psi.FileName = "C:\\Program Files\\MMX\\run_node.cmd";
            consoleControl1.StartProcess(psi);
            //consoleControl1.StartProcess("cmd", null);
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

            if (consoleControl1.IsProcessRunning)
            {
                //consoleControl1.ProcessInterface.Process.CloseMainWindow();
                consoleControl1.StopProcess();
            }

        }

    }


}
