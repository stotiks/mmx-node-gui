using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common.Plotter;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
    /// <summary>
    /// Interaction logic for MultiDir.xaml
    /// </summary>
    public partial class MultiFolder
    {
        public class Dir : INotifyPropertyChanged
        {
            string _path = "";
            public string Path
            {
                get => _path;
                set
                {
                    _path = value;
                    NotifyPropertyChanged();
                }
            }


            bool _isFirst;
            public bool IsFirst
            {
                get => _isFirst;
                internal set
                {
                    _isFirst = value;
                    NotifyPropertyChanged();
                }
            }

            bool _isAlone;
            public bool IsAlone
            {
                get => _isAlone;
                internal set
                {
                    _isAlone = value;
                    NotifyPropertyChanged();
                }
            }
            public Dir()
            {
            }

            public Dir(string path)
            {
                Path = path;
            }

            bool _isLast;
            public bool IsLastAndNotEmpty
            {
                get => _isLast;
                internal set
                {
                    _isLast = value;
                    NotifyPropertyChanged();
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }

        ObservableCollection<Dir> _finalDirs;
        public ObservableCollection<Dir> FinalDirs => _finalDirs;

        public PlotterOptions PlotterOptions => PlotterOptions.Instance;
        public MultiFolder()
        {
            InitializeComponent();
            DataContext = this;

            _finalDirs = new ObservableCollection<Dir>(PlotterOptions.Instance.multifinaldir.Value.Select(path => new Dir(path)).ToList());
                        
            FinalDirs.CollectionChanged += (o, e) => UpdateItems();

            if (FinalDirs.Count == 0)
            {
                AddDir(new Dir());
            }

            RecalcItems();

            MultiFolderItemsControl.ItemsSource = FinalDirs;

            PlotterOptions.Instance.finaldir.PropertyChanged += (o, e) => FinalDirs.First().Path = PlotterOptions.Instance.finaldir.Value;
        }

        private void UpdateItems()
        {
            RecalcItems();
            PlotterOptions.Instance.multifinaldir.Value = FinalDirs.Select(x => x.Path).ToList();
        }

        private void RecalcItems()
        {
            for (int i = 0; i < FinalDirs.Count; i++)
            {
                Dir dir = FinalDirs[i];
                dir.PropertyChanged -= DirPathPropertyChanged;
                dir.PropertyChanged += DirPathPropertyChanged;

                dir.IsAlone = FinalDirs.Count == 1;
                dir.IsFirst = i == 0;
                dir.IsLastAndNotEmpty = (i == FinalDirs.Count - 1) && !string.IsNullOrEmpty(dir.Path);
            }
        }

        private void DirPathPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Dir.Path))
            {
                var dir = (sender as Dir);
                if (dir.IsFirst)
                {
                    PlotterOptions.Instance.finaldir.Value = dir.Path;
                }
                UpdateItems();
            }
        }

        private void ChooseFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var dir = button.DataContext as Dir;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(dir.Path) ? "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}" : dir.Path,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                dir.Path = PlotterOptionsHelpers.FixDir(dialog.FileName);
            }
        }

        private void AddFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string path;
            var button = sender as Button;

            var dir = button.DataContext as Dir;
            path = dir.Path;
            if (!string.IsNullOrEmpty(path))
            {
                AddDir(new Dir());
            }
        }

        private void RemoveFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var dir = button.DataContext as Dir;

            if(FinalDirs.Count > 1)
            {
                RemoveDir(dir);                
            }            
        }

        private void AddDir(Dir dir)
        {
            FinalDirs.Add(dir);
            dir.PropertyChanged += DirPathPropertyChanged;
        }

        private void RemoveDir(Dir dir)
        {
            FinalDirs.Remove(dir);
            dir.PropertyChanged -= DirPathPropertyChanged;
        }
    }
}
