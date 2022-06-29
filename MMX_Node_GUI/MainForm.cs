using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{

    public partial class MainForm : MaterialForm
    {

        private bool disableCloseToNotification = false;

        public bool CloseToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.closeToNotification && !disableCloseToNotification;

        public bool MinimizeToNotification => Properties.Settings.Default.showInNotifitation && Properties.Settings.Default.minimizeToNotification;

        public MainForm()
        {

            InitializeSkinManager();

            InitializeComponent();

            InitializeLocalization();

            this.imageList1.Images.Add("database-sync", Properties.Resources.database_sync);
            this.imageList1.Images.Add("tractor", Properties.Resources.tractor);
            this.imageList1.Images.Add("harddisk-plus", Properties.Resources.harddisk_plus);
            this.imageList1.Images.Add("cog", Properties.Resources.cog);
            this.imageList1.Images.Add("information", Properties.Resources.information);

            menuMaterialTabControl.ImageList = imageList1;

            menuMaterialTabControl.TabPages[0].ImageKey = "database-sync";
            menuMaterialTabControl.TabPages[1].ImageKey = "tractor";
            menuMaterialTabControl.TabPages[2].ImageKey = "harddisk-plus";
            menuMaterialTabControl.TabPages[3].ImageKey = "cog";
            menuMaterialTabControl.TabPages[4].ImageKey = "information";


            InitializeNode();

            InitializeSettings();

            InitializeHarvester();

            InitializePlotter();

            this.notifyIcon1.Text = this.Text;

            this.menuMaterialTabControl.Controls.Remove(this.aboutTabPage);
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
                                                                   this.Text,
                                                                   Properties.Resources.closeQuestion,
                                                                   Properties.Resources.no, true, Properties.Resources.yes);
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
        
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Show();
                WindowState = FormWindowState.Normal;
                NativeMethods.SetForegroundWindow(this.Handle);
            }
            base.WndProc(ref message);
        }

    }
}
