using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common.Plotter;
using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using static Mmx.Gui.Win.Common.Plotter.PlotterOptions;

namespace Mmx.Gui.Win.Wpf.Common.Pages
{
    /// <summary>
    /// Interaction logic for PlotterPage.xaml
    /// </summary>
    ///     
    public partial class PlotterPage: INotifyPropertyChanged
    {
        public PlotterOptions PlotterOptions => PlotterOptions.Instance;

        public PlotterPage(bool IsMmxOnly = true)
        {
            PlotterOptions.IsMmxOnly = IsMmxOnly;

            InitializeComponent();
            DataContext = this;

            if (PlotterOptions.IsMmxOnly)
            {
                //NFT PLOTS DISABLED
                createPlotNFT.IsEnabled = false;
            }

            PlotterOptions.plotter.PropertyChanged += (o, e) => UpdatePlotterUrl();

        }

        private void ChooseFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var property = typeof(PlotterOptions).GetProperty(button.Tag as string);
            dynamic item = property.GetValue(PlotterOptions.Instance);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(item.Value) ? "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}" : item.Value,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                item.Value = PlotterOptionsHelpers.FixDir(dialog.FileName);
            }
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlotterDialog.ShowAsync(ContentDialogPlacement.InPlace);
            PlotterDialog.StartPlotter();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string PlotterUrl
        {
            get {
                var value = PlotterOptions.plotter.Value;
                Plotters plotterEnum = (Plotters)Enum.ToObject(typeof(Plotters), value);

                var memberInfo = typeof(Plotters).GetMember(plotterEnum.ToString())[0];
                var urlAttribute = memberInfo.GetCustomAttribute<UrlAttribute>();
                return urlAttribute?.Url;
            }
        }

        public bool PlotterUrlIsVisible => PlotterUrl != null;
        public bool BucketsIsVisible => PlotterOptions.buckets.IsVisible || PlotterOptions.buckets3.IsVisible || PlotterOptions.chunksize.IsVisible;

        public void UpdatePlotterUrl()
        {
            NotifyPropertyChanged(nameof(PlotterUrl));
            NotifyPropertyChanged(nameof(PlotterUrlIsVisible));
        }

    }
}
