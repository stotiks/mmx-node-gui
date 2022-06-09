using CefSharp;
using CefSharp.Handler;
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
        static private string logoutJS = GetResource("logout.js");
        static private string waitStartJS = GetResource("waitStart.js");
        static private string loadingHtml = GetResource("loading.html");
        static private string jsString = "//javascript";
        static private string logoutHtml = loadingHtml.Replace(jsString, logoutJS);
        static private string waitStartHtml = loadingHtml.Replace(jsString, waitStartJS);

        ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser()
        {
            ActivateBrowserOnCreation = true,
            Dock = DockStyle.Fill,
            Location = new Point(0, 0)
        };

        private readonly Node node = new Node();

        private void InitializeNode()
        {

            chromiumWebBrowser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            CefSharpSettings.WcfEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            var boundObject = new MMXBoundObject(this);
            chromiumWebBrowser.JavascriptObjectRepository.Register("mmx", boundObject, isAsync: false, options: BindingOptions.DefaultBinder);

            chromiumWebBrowser.MenuHandler = new SearchContextMenuHandler();
            chromiumWebBrowser.RequestHandler = new CustomRequestHandler();

            nodeTabPage.Controls.Add(chromiumWebBrowser);

            //node.Started += (sender, e) => chromiumWebBrowser.LoadUrl(Node.guiUri.ToString());
            node.BeforeStarted += (sender, e) => chromiumWebBrowser.LoadHtml(waitStartHtml, Node.baseUri.ToString());
            node.BeforeStop += (sender, e) => chromiumWebBrowser.LoadHtml(logoutHtml, Node.baseUri.ToString());

        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var browserHost = chromiumWebBrowser.GetBrowser().GetHost();
            var requestContext = browserHost.RequestContext;
            string errorMessage = "";

            var x = requestContext.GetAllPreferences(true);
            requestContext.SetPreference("intl.accept_languages", Properties.Settings.Default.langCode, out errorMessage);
        }


        public class XApiTokenResourceRequestHandler : ResourceRequestHandler
        {

            protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            {
                var headers = request.Headers;
                headers[Node.XApiTokenName] = Node.XApiToken;
                request.Headers = headers;

                return CefReturnValue.Continue;
            }
        }

        public class CustomRequestHandler : RequestHandler
        {
            protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
            {

                if ( request.Url.StartsWith(Node.apiUri.ToString()) ||
                     request.Url.StartsWith(Node.wapiUri.ToString())
                  )
                {
                    return new XApiTokenResourceRequestHandler();
                } else
                {
                    return base.GetResourceRequestHandler(chromiumWebBrowser, browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
                }
            }

        }

        private void MainForm_Node_Load()
        {
            Task.Run(() => node.Start());
        }

        private void MainForm_Node_FormClosing()
        {
            nodeTabPage.Show();
            node.Stop();
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
                model.AddItem(CefMenuCommand.Reload, Properties.Resources.reload);
#if DEBUG
                model.AddSeparator();
                model.AddItem((CefMenuCommand)26501, "Show DevTools");
#endif
            }

            public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                CefMenuCommand commandId, CefEventFlags eventFlags)
            {

                if (commandId == (CefMenuCommand)26501)
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
