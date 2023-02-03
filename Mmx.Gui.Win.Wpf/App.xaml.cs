using Mmx.Gui.Win.Common.Cef;
using System;

namespace Mmx.Gui.Win.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App() : base()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CefUtils.CurrentDomain_AssemblyResolve;
        }

    }

}
