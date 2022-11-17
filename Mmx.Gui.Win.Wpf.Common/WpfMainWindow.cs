using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;

namespace Mmx.Gui.Win.Wpf.Common
{
    abstract public class WpfMainWindow: Window
    {
        protected bool closeCancel = true;
        protected bool closePending = false;
        protected bool disableCloseToNotification = false;

        public WpfMainWindow()
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
            StateChanged += Window_StateChanged;
            Closing += Window_Closing;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (Settings.MinimizeToNotification &&
                WindowState == WindowState.Minimized)
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    Hide();
                }));
            }
        }

        public void Restore()
        {
            if (!closePending)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            NativeMethods.SetForegroundWindow(new WindowInteropHelper(this).Handle);
        }

        protected void contentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Restore();
            }

            return IntPtr.Zero;
        }

        protected async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs args)
        {

            if (Settings.CloseToNotification && !disableCloseToNotification)
            {
                Hide();
                args.Cancel = true;
                return;
            }

            if (!closePending)
            {
                args.Cancel = true;
                closePending = true;

                await Task.Yield();
                Restore();

                if (await CanClose())
                {
                    await OnBeforeClose();
                    Close();
                }

                closePending = false;
            }

        }

        private async Task OnBeforeClose()
        {
            if (!(BeforeClose is null)) await BeforeClose(this, EventArgs.Empty);
        }

        public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);
        public event AsyncEventHandler<EventArgs> BeforeClose;

        protected virtual async Task<bool> CanClose()
        {
            await Task.Yield();
            return true;
        }

        internal new void Close()
        {
            disableCloseToNotification = true;
            base.Close();
            disableCloseToNotification = false;
        }

    }
}
