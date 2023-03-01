using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;

namespace Mmx.Gui.Win.Wpf.Common
{
    public abstract class WpfMainWindow: Window
    {
        private bool _closePending;
        private bool _disableCloseToNotification;

        protected WpfMainWindow()
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
            StateChanged += WpfMainWindow_StateChanged;
            base.Closing += Window_Closing;
            //BeforeClose += async (o, e) => await Task.Run(Settings.Default.Save);
        }

        private void WpfMainWindow_StateChanged(object sender, EventArgs e)
        {
            if (Settings.MinimizeToNotification &&
                WindowState == WindowState.Minimized)
            {
                Dispatcher.BeginInvoke(new Action(Hide));
            }
        }

        public void Restore()
        {
            if (!_closePending)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            NativeMethods.SetForegroundWindow(new WindowInteropHelper(this).Handle);
        }

        protected void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
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

        private async void Window_Closing(object sender, CancelEventArgs args)
        {

            if (Settings.CloseToNotification && !_disableCloseToNotification)
            {
                Hide();
                args.Cancel = true;
                return;
            }

            if (!_closePending)
            {
                args.Cancel = true;
                _closePending = true;

                await Task.Yield();
                Restore();

                var cancelEventArgs = new CancelEventArgs();
                if (!(Closing is null)) await Closing(this, cancelEventArgs);

                if(cancelEventArgs.Cancel == false)
                {
                    await OnBeforeClose();
                    Close();
                }

                _closePending = false;
            }

        }

        private async Task OnBeforeClose()
        {
            if (!(BeforeClose is null)) await BeforeClose(this, EventArgs.Empty);
        }

        public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);
        public event AsyncEventHandler<EventArgs> BeforeClose;
        public new event AsyncEventHandler<CancelEventArgs> Closing;

        internal new void Close()
        {
            _disableCloseToNotification = true;
            base.Close();
            _disableCloseToNotification = false;
        }

    }
}
