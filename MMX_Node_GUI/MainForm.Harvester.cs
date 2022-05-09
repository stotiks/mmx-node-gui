using MaterialSkin;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private JObject harvesterConfig;

        private void InitializeHarvester()
        {
            string harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
            harvesterConfig = JObject.Parse(harvesterJson);

            //int num_threads = harvesterConfig.Value<int>("num_threads");
            //int reload_interval = harvesterConfig.Value<int>("reload_interval");
            JArray plot_dirs = harvesterConfig.Value<JArray>("plot_dirs");

            for (int i = 0; i < plot_dirs.Count; i++)
            {
                var item = new MaterialListBoxItem(plot_dirs[i].ToString());
                plotFoldersMaterialListBox.Items.Add(item);
            }
        }

        private void addPlotFolderMaterialButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var item = new MaterialListBoxItem(dialog.FileName);
                plotFoldersMaterialListBox.Items.Add(item);

                SaveHavesterConfig();
            }
        }

        private void removePlotFolderMaterialButton_Click(object sender, EventArgs e)
        {
            plotFoldersMaterialListBox.Items.Remove(plotFoldersMaterialListBox.SelectedItem);
            SaveHavesterConfig();
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
