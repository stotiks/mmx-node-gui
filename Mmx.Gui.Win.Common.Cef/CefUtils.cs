using CefSharp;
using CefSharp.Handler;
using Mmx.Gui.Win.Common.Node;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Cef
{
    public static class CefUtils
    {
        // The subfolder, where the cefsharp files will be moved to
        private static readonly string cefSubFolder = @"gui\cefsharp";
        // If the assembly resolver loads cefsharp from another folder, set this to true
        private static bool _resolved;


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InitializeCefSharp(CefSettingsBase settings)
        {
            // Set BrowserSubProcessPath when cefsharp moved to the subfolder
            if (_resolved)
            {
                settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cefSubFolder, "CefSharp.BrowserSubprocess.exe");
            }

            //settings.UserAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36 (mmx.gui.win)";
            settings.Locale = Properties.Settings.Default.LanguageCode;

            if (Properties.Settings.Default.CEF_GPU_Disabled)
            {
                settings.CefCommandLineArgs.Add("disable-gpu", "1");
                settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
                settings.CefCommandLineArgs.Add("disable-gpu-process", "1");
                settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            }

            // Make sure you set performDependencyCheck false
            CefSharp.Cef.Initialize(settings, performDependencyCheck: false);
        }

        /// <summary>
        /// Will attempt to load missing assemblys from subfolder
        /// </summary>
        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                _resolved = true; // Set to true, so BrowserSubprocessPath will be set

                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string subfolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cefSubFolder, assemblyName);
                return File.Exists(subfolderPath) ? Assembly.LoadFile(subfolderPath) : null;
            }

            return null;
        }


        private class XApiTokenResourceRequestHandler : ResourceRequestHandler
        {

            protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            {
                request.SetHeaderByName(NodeApi.XApiTokenName, NodeApi.XApiToken, true);
                //request.SetHeaderByName("User-Agent", request.GetHeaderByName("User-Agent") + " (mmx.gui.win)", true);

                return CefReturnValue.Continue;
            }
        }

        public class CustomRequestHandler : RequestHandler
        {
            protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
            {

                if ( request.Url.StartsWith(NodeApi.baseUri.ToString()) 
                    && request.Url != NodeApi.dummyUri.ToString() )
                {
                    return new XApiTokenResourceRequestHandler();
                }
                else
                {
                    return base.GetResourceRequestHandler(chromiumWebBrowser, browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
                }
            }

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
                //TODO
                model.AddItem(CefMenuCommand.Reload, //Properties.Resources.reload
                                                     "Reload");
//#if DEBUG
                model.AddSeparator();
                model.AddItem((CefMenuCommand)26501, "Show DevTools");
//#endif
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
    }
}
