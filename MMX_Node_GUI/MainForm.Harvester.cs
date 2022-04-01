using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private JObject harvesterConfig;

        private void InitializeHarvester()
        {

            this.menuMaterialTabControl.SelectedIndexChanged += new System.EventHandler(this.MenuMaterialTabControl_SelectedIndexChanged);
        }

        private void IsNotNull(object sender, ConvertEventArgs e)
        {
            e.Value = e.Value != null && (int)e.Value >= 0 ? true : false;
        }

        private void MenuMaterialTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.menuMaterialTabControl.SelectedTab == harvesterTabPage)
            {
                string harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
                harvesterConfig = JObject.Parse(harvesterJson);

                //int num_threads = harvesterConfig.Value<int>("num_threads");
                //int reload_interval = harvesterConfig.Value<int>("reload_interval");
                JArray plot_dirs = harvesterConfig.Value<JArray>("plot_dirs");

                plotFoldersMaterialListBox.Items.Clear();
                for (int i = 0; i < plot_dirs.Count; i++)
                {
                    var item = new MaterialListBoxItem(plot_dirs[i].ToString());
                    plotFoldersMaterialListBox.Items.Add(item);
                }
            }
        }

        private void addPlotFolderMaterialButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dirName = dialog.FileName;

                Node.AddPlotDirTask(dirName).ContinueWith(task => ShowSnackBar(Properties.Resources.dirAdded));

                plotFoldersMaterialListBox.Items.Add(new MaterialListBoxItem(dirName));
               
                SaveHavesterConfig();
            }
        }

        private void removePlotFolderMaterialButton_Click(object sender, EventArgs e)
        {
            if (plotFoldersMaterialListBox.SelectedItem == null)
            {
                return;
            }

            var dirName = plotFoldersMaterialListBox.SelectedItem.Text;
            Node.RemovePlotDirTask(dirName).ContinueWith(task => ShowSnackBar(Properties.Resources.dirRemoved));

            plotFoldersMaterialListBox.Items.Remove(plotFoldersMaterialListBox.SelectedItem);            

            SaveHavesterConfig();
        }

        private void ShowSnackBar(string message)
        {
            if (ControlInvokeRequired(this, () => ShowSnackBar(message))) return;
            var SnackBarMessage = new MaterialSnackBar(message, Properties.Resources.ok, true);
            SnackBarMessage.Show(this);
        }

        private void SaveHavesterConfig()
        {
            ((JArray)harvesterConfig["plot_dirs"]).Clear();
            for (int i = 0; i < plotFoldersMaterialListBox.Items.Count; i++)
            {
                ((JArray)harvesterConfig["plot_dirs"]).Add(plotFoldersMaterialListBox.Items[i].Text);
            }
            File.WriteAllText(Node.harvesterConfigPath, harvesterConfig.ToString());
        }
    }
}
