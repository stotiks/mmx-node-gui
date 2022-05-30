﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private PlotterOptions plotterOptions = new PlotterOptions();

        private void InitializePlotter()
        {

#if !DEBUG
            this.MenuMaterialTabControl.Controls.Remove(this.plotterTabPage);
#endif

            farmerkeyMaterialTextBox2.DataBindings.Add("Text", plotterOptions.farmerkey, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            farmerkeyMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.farmerkey, "Description");

            poolkeyMaterialTextBox2.DataBindings.Add("Text", plotterOptions.poolkey, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            poolkeyMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.poolkey, "Description");

            contractMaterialTextBox2.DataBindings.Add("Text", plotterOptions.contract, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            contractMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.contract, "Description");

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

            //tmpdirMaterialTextBox2.DataBindings.Add("Text", plotterOptions.tmpdir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmpdirMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.tmpdir, "Description");
            tmpdirMaterialButton.Tag = tmpdirMaterialTextBox2;


            Binding tmpdir2Bind = new Binding("Text", plotterOptions.tmpdir2, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmpdir2Bind.Format += FixDir2;
            tmpdir2Bind.Parse += FixDir2;
            tmpdir2MaterialTextBox2.DataBindings.Add(tmpdir2Bind);

            //tmpdir2MaterialTextBox2.DataBindings.Add("Text", plotterOptions.tmpdir2, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmpdir2MaterialTextBox2.DataBindings.Add("Hint", plotterOptions.tmpdir2, "Description");
            tmpdir2MaterialButton.Tag = tmpdir2MaterialTextBox2;


            Binding finaldirBind = new Binding("Text", plotterOptions.finaldir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            finaldirBind.Format += FixDir2;
            finaldirBind.Parse += FixDir2;
            finaldirMaterialTextBox2.DataBindings.Add(finaldirBind);

            //finaldirMaterialTextBox2.DataBindings.Add("Text", plotterOptions.finaldir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            finaldirMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.finaldir, "Description");
            finaldirMaterialButton.Tag = finaldirMaterialTextBox2;

            Binding stagedirBind = new Binding("Text", plotterOptions.stagedir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            stagedirBind.Format += FixDir2;
            stagedirBind.Parse += FixDir2;
            stagedirMaterialTextBox2.DataBindings.Add(stagedirBind);

            //stagedirMaterialTextBox2.DataBindings.Add("Text", plotterOptions.stagedir, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            stagedirMaterialTextBox2.DataBindings.Add("Hint", plotterOptions.stagedir, "Description");
            stagedirMaterialButton.Tag = stagedirMaterialTextBox2;


            //------
            waitforcopyMaterialSwitch.DataBindings.Add("Checked", plotterOptions.waitforcopy, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            waitforcopyMaterialSwitch.DataBindings.Add("Text", plotterOptions.waitforcopy, "Description");

            tmptoggleMaterialSwitch.DataBindings.Add("Checked", plotterOptions.tmptoggle, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            tmptoggleMaterialSwitch.DataBindings.Add("Text", plotterOptions.tmptoggle, "Description");

            directoutMaterialSwitch.DataBindings.Add("Checked", plotterOptions.directout, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            directoutMaterialSwitch.DataBindings.Add("Text", plotterOptions.directout, "Description");

            nftplotMaterialSwitch.DataBindings.Add("Checked", plotterOptions.nftplot, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            nftplotMaterialSwitch.DataBindings.Add("Text", plotterOptions.nftplot, "Description");


            countMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.count, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            countMaterialLabel.DataBindings.Add("Text", plotterOptions.count, "Description");

            sizeMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.size, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            sizeMaterialLabel.DataBindings.Add("Text", plotterOptions.size, "Description");

            //------
            threadsMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.threads, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            threadsMaterialLabel.DataBindings.Add("Text", plotterOptions.threads, "Description");

            rmulti2MaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.rmulti2, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            rmulti2MaterialLabel.DataBindings.Add("Text", plotterOptions.rmulti2, "Description");

            bucketsMaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.buckets, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            bucketsMaterialLabel.DataBindings.Add("Text", plotterOptions.buckets, "Description");

            buckets3MaterialNumericUpDown.DataBindings.Add("Value", plotterOptions.buckets3, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            buckets3MaterialLabel.DataBindings.Add("Text", plotterOptions.buckets3, "Description");



            //------
            materialMultiLineTextBox21.DataBindings.Add("Text", plotterOptions, "PloterArgs");

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

    }
}