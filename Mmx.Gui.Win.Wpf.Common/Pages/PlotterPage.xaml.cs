using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common.Plotter;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Pages
{
    /// <summary>
    /// Interaction logic for PlotterPage.xaml
    /// </summary>
    ///     
    public partial class PlotterPage
    {
        public PlotterOptions PlotterOptions => PlotterOptions.Instance;

        public PlotterPage(bool isMmx = true)
        {
            PlotterOptions.IsMmx = isMmx;

            InitializeComponent();
            DataContext = this;

            if (PlotterOptions.IsMmx)
            {
                //NFT PLOTS DISABLED
                PlotterOptions.Instance.nftplot.Value = false;
                createPlotNFT.IsEnabled = false;
            }

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

    }
}
