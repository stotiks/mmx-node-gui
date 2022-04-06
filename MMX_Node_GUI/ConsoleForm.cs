using System.Windows.Forms;

namespace MMX_NODE_GUI
{

    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();

            this.consoleControl.InternalRichTextBox.HideSelection = false;

            WindowState = FormWindowState.Minimized;
            base.Show();
            Hide();           
        }

        public new void Show()
        {
            FormExtensions.Restore(this);
            base.Show();
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
