using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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

        public static readonly DependencyProperty DirectoriesProperty =
           DependencyProperty.Register(nameof(Directories), typeof(List<string>), typeof(MultiFolder),
               new FrameworkPropertyMetadata(new List<string>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, e) => ((MultiFolder)o).OnDirectoriesPropertyChanged()));

        private void OnDirectoriesPropertyChanged()
        {
            FinalDirs.CollectionChanged -= NewMethod();
            FinalDirs.Clear();
            FinalDirs.AddRange(Directories.Select(path => new Dir(path)).ToList());
            RecalcItems();
            FinalDirs.CollectionChanged += NewMethod();
        }

        private NotifyCollectionChangedEventHandler NewMethod()
        {
            return (o, e) => UpdateItems();
        }

        public List<string> Directories {
            get { 
                return (List<string>)GetValue(DirectoriesProperty); 
            }
            set {
                if ( value != (List<string>)GetValue(DirectoriesProperty) )
                {
                    SetValue(DirectoriesProperty, value);
                }
                
            }
        }

        public ObservableCollection<Dir> FinalDirs = new ObservableCollection<Dir>();

        public MultiFolder()
        {
            InitializeComponent();

            if (FinalDirs.Count == 0)
            {
                AddDir(new Dir());
            }

            RecalcItems();

            MultiFolderItemsControl.ItemsSource = FinalDirs;

            //TODO
            //PlotterOptions.Instance.finaldir.PropertyChanged += (o, e) => FinalDirs.First().Path = PlotterOptions.Instance.finaldir.Value;
        }

        private void UpdateItems()
        {
            RecalcItems();
            Directories = FinalDirs.Select(x => x.Path).ToList();
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
                    //TODO
                    //PlotterOptions.Instance.finaldir.Value = dir.Path;
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
                dir.Path = Win.Common.Plotter.PlotterOptionsHelpers.FixDir(dialog.FileName);
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

    internal static class ObservableCollectionExtensions
    {

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items) => items.ToList().ForEach(collection.Add);
    }
}
