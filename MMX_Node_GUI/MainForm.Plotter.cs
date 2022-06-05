using MaterialSkin.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private PlotterOptions plotterOptions = new PlotterOptions();

        private void InitializePlotter()
        {
                  
            farmerkeyMaterialTextBox2.DataBindings.Add("Text", plotterOptions.farmerkey, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            poolkeyMaterialTextBox2.DataBindings.Add("Text", plotterOptions.poolkey, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            contractMaterialTextBox2.DataBindings.Add("Text", plotterOptions.contract, "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            Binding ogPlotBind = new Binding("Enabled", plotterOptions.nftplot, "Value");
            ogPlotBind.Format += SwitchBool;
            ogPlotBind.Parse += SwitchBool;
            poolkeyMaterialTextBox2.DataBindings.Add(ogPlotBind);

            contractMaterialTextBox2.DataBindings.Add("Enabled", plotterOptions.nftplot, "Value");

            //------

            Binding tmpdirBind = new Binding("Text", plotterOptions.tmpdir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmpdirBind.Format += FixDir2;
            tmpdirBind.Parse += FixDir2;
            tmpdirMaterialTextBox2.DataBindings.Add(tmpdirBind);

            tmpdirMaterialButton.Tag = tmpdirMaterialTextBox2;


            Binding tmpdir2Bind = new Binding("Text", plotterOptions.tmpdir2, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmpdir2Bind.Format += FixDir2;
            tmpdir2Bind.Parse += FixDir2;
            tmpdir2MaterialTextBox2.DataBindings.Add(tmpdir2Bind);

            tmpdir2MaterialButton.Tag = tmpdir2MaterialTextBox2;


            Binding finaldirBind = new Binding("Text", plotterOptions.finaldir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            finaldirBind.Format += FixDir2;
            finaldirBind.Parse += FixDir2;
            finaldirMaterialTextBox2.DataBindings.Add(finaldirBind);

            finaldirMaterialButton.Tag = finaldirMaterialTextBox2;


            Binding stagedirBind = new Binding("Text", plotterOptions.stagedir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            stagedirBind.Format += FixDir2;
            stagedirBind.Parse += FixDir2;
            stagedirMaterialTextBox2.DataBindings.Add(stagedirBind);


            stagedirMaterialButton.Tag = stagedirMaterialTextBox2;

            //------
            waitforcopyMaterialSwitch.DataBindings.Add("Checked", plotterOptions.waitforcopy, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmptoggleMaterialSwitch.DataBindings.Add("Checked", plotterOptions.tmptoggle, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            directoutMaterialSwitch.DataBindings.Add("Checked", plotterOptions.directout, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            nftplotMaterialSwitch.DataBindings.Add("Checked", plotterOptions.nftplot, "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            countMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.count, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            sizeMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.size, "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            //------
            threadsMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.threads, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            rmulti2MaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.rmulti2, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            bucketsMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.buckets, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            buckets3MaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.buckets3, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            //------
            materialMultiLineTextBox21.DataBindings.Add("Text", plotterOptions, "PlotterCmd");

        }

        private void FixDir2(object sender, ConvertEventArgs e)
        {
            e.Value = FixDir((string)e.Value);
        }


        private void SwitchBool(object sender, ConvertEventArgs e)
        {
            e.Value = !((bool)e.Value);
        }

        private string FixDir(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return "";

            dir = dir.Replace('/', '\\');

            if (dir.Length > 0 && dir.Last() != '\\')
            {
                dir += '\\';
            }

            return dir;
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            dynamic textBox = (sender as Control).Tag;

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(textBox.Text) ? "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}" : textBox.Text;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox.Text = FixDir(dialog.FileName);
            }
        }

        public bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }

        public void EnableStartButton()
        {
            if (ControlInvokeRequired(startMaterialButton, () => EnableStartButton())) return;
            startMaterialButton.Enabled = true;
        }

        private void startMaterialButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Node.workingDirectory;
            processStartInfo.FileName = plotterOptions.PlotterExe;
            processStartInfo.Arguments = plotterOptions.PlotterArguments;

            var process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;

            process.Exited += (sender1, e1) => EnableStartButton();

            var processStarted = process.Start();
            startMaterialButton.Enabled = false;
        }

        internal void CopyKeysToPlotter(string json)
        {
            if (ControlInvokeRequired(menuMaterialTabControl, () => CopyKeysToPlotter(json))) return;
            menuMaterialTabControl.SelectTab(plotterTabPage);
            plotterSettingsMaterialTabControl.SelectedTab = keysTabPage;

            dynamic keys = JsonConvert.DeserializeObject(json);
            farmerkeyMaterialTextBox2.Text = keys["farmer_public_key"];
            nftplotMaterialSwitch.Checked = false;
            poolkeyMaterialTextBox2.Text = keys["pool_public_key"];

            MaterialSnackBar SnackBarMessage = new MaterialSnackBar("Keys copied succesfully", "OK", true);
            SnackBarMessage.Show(this);
        }

    }
}
