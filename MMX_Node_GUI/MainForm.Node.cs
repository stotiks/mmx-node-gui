using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        static private string loginJS = GetResource("login.js"); 
        static private string logoutJS = GetResource("logout.js");
        static private string loadingHtml = GetResource("loading.html");
        static private string jsString = "//javascript";
        static private string loginHtml = loadingHtml.Replace(jsString, loginJS.Replace("_mmx_password_", Node.GetPassword()));
        static private string logoutHtml = loadingHtml.Replace(jsString, logoutJS);

        ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser()
        {
            ActivateBrowserOnCreation = true,
            Dock = DockStyle.Fill,
            Location = new Point(0, 0)
        };

        private readonly Node node = new Node();

        private void InitializeNode()
        {

            // alert(window.mmx.myProperty);
            // var message = "message"
            // window.mmx.postMessage(message);

            CefSharpSettings.WcfEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;

            var boundObject = new MMXBoundObject();
            //boundObject.Password = Node.GetPassword();

            chromiumWebBrowser.JavascriptObjectRepository.Register("mmx", boundObject, isAsync: false, options: BindingOptions.DefaultBinder);

            chromiumWebBrowser.MenuHandler = new SearchContextMenuHandler();
            

            nodeTabPage.Controls.Add(chromiumWebBrowser);
            //node.Started += (sender, e) => chromiumWebBrowser.LoadUrl(Node.guiUri.ToString());
            //node.BeforeStop += (sender, e) => chromiumWebBrowser.LoadHtml(loadingHtml, Node.baseUri.ToString());

        }

        private void NodeMainForm_Load()
        {
            Task.Run(async () => await chromiumWebBrowser.LoadHtmlAsync(loginHtml, Node.baseUri.ToString())).Wait();            
            node.Start();
        }

        public class SearchContextMenuHandler : IContextMenuHandler
        {
            //This method prepares the context menu
            public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
            {
                //model.Clear();
                //model.AddItem(CefMenuCommand.Back, "Back");
                //model.AddItem(CefMenuCommand.Forward, "Forward");
                model.AddSeparator();
                model.AddItem(CefMenuCommand.Reload, "Reload");
#if DEBUG
                model.AddSeparator();
                model.AddItem(CefMenuCommand.CustomFirst, "Show DevTools");
#endif
            }

            public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                CefMenuCommand commandId, CefEventFlags eventFlags)
            {

                if (commandId == CefMenuCommand.CustomFirst)
                {
                    browserControl.ShowDevTools();
                }

                return false;
            }

            public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
            {
            }

            public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
            {
                return false;
            }
        }

        private static string GetResource(string resName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resName));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }


    }
}
