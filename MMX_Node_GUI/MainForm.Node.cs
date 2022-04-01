using CefSharp;
using CefSharp.WinForms;
using Mmx.Gui.Win.Common;
using System.Drawing;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {

        ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser()
        {
            ActivateBrowserOnCreation = true,
            Dock = DockStyle.Fill,
            Location = new Point(0, 0)
        };

        private readonly Node node = new Node();
        private readonly MMXBoundObject boundObject = new MMXBoundObject();
        private UpdateChecker updateChecker;

        private void InitializeNode()
        {

            CefSharpSettings.WcfEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            
            boundObject.KeysToPlotter += (json) => CopyKeysToPlotter(json);
            chromiumWebBrowser.JavascriptObjectRepository.Register("mmx", boundObject, isAsync: false, options: BindingOptions.DefaultBinder);

            chromiumWebBrowser.MenuHandler = new CefUtils.SearchContextMenuHandler();
            chromiumWebBrowser.RequestHandler = new CefUtils.CustomRequestHandler();

            nodeTabPage.Controls.Add(chromiumWebBrowser);

            //node.Started += (sender, e) => chromiumWebBrowser.LoadUrl(Node.loadhtmlUri.ToString());
            node.BeforeStarted += (sender, e) => chromiumWebBrowser.LoadHtml(Node.waitStartHtml, Node.dummyUri.ToString());
            node.BeforeStop += (sender, e) => chromiumWebBrowser.LoadHtml(Node.logoutHtml, Node.dummyUri.ToString());

            versionToolStripStatusLabel.Text = Node.VersionTag;
            versionToolStripStatusLabel.Visible = true;

            node.Started += (sender, e) =>
            {
                updateChecker = new UpdateChecker();
                updateChecker.UpdateAvailable += (o, e1) =>
                {

                    BeginInvoke(new MethodInvoker(delegate
                    {
                        newVersionToolStripStatusLabel.Visible = true;
                    }));

                };
                updateChecker.CheckAsync();
            };


        }

        private void MainForm_Node_Load()
        {
            node.StartAsync();
        }

        private void MainForm_Node_FormClosing()
        {
            nodeTabPage.Show();
            node.Stop();
        }

    }
}
