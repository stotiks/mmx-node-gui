using CefSharp;
using System;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{

    public static class WebBrowserExtensions
    {
        public static Task LoadPageAsync(this IWebBrowser browser, string address = null)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Handler(object sender, LoadingStateChangedEventArgs args)
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= Handler;
                    //Important that the continuation runs async using TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResult(true);
                }
            }

            browser.LoadingStateChanged += Handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }

            return tcs.Task;
        }
        public static Task LoadHtmlAsync(this IWebBrowser browser, string html, string address)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Handler(object sender, LoadingStateChangedEventArgs args)
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= Handler;
                    //Important that the continuation runs async using TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResult(true);
                }
            }

            browser.LoadingStateChanged += Handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.LoadHtml(html, address);
            }

            return tcs.Task;
        }

    }
}
