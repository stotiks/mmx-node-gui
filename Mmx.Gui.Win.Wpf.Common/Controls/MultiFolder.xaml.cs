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
        class DirObservableCollection : ObservableCollection<Dir>, INotifyPropertyChanged
        {
            public DirObservableCollection() : base()
            {
                CollectionChanged += RecalcItems;
            }

            public DirObservableCollection(List<Dir> list) : base(list)
            {
                CollectionChanged += RecalcItems;
            }

            protected override void ClearItems()
            {
                this.ToList().ForEach(item => item.PropertyChanged -= ItemPropertyChanged);
                base.ClearItems();
            }

            protected override void InsertItem(int index, Dir item)
            {
                item.PropertyChanged += ItemPropertyChanged;
                base.InsertItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                this[index].PropertyChanged -= ItemPropertyChanged;
                base.RemoveItem(index);
            }

            private void RecalcItems(object sender, NotifyCollectionChangedEventArgs e)
            {
                RecalcItems();
            }

            private void RecalcItems()
            {
                for (int i = 0; i < Count; i++)
                {
                    Dir dir = this[i];

                    dir.IsAlone = Count == 1;
                    dir.IsFirst = i == 0;
                    dir.IsLastAndNotEmpty = (i == Count - 1) && !string.IsNullOrEmpty(dir.Path);
                }
            }

            private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(Dir.Path))
                {
                    var dir = (sender as Dir);
                    if (dir.IsFirst)
                    {
                        NotifyItemChanged("FirstItem");
                    }
                    RecalcItems();
                    OnCollectionChanged(NotifyCollectionChangedAction.Add, this[0], 0); //TODO
                }
            }

            private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
            }

            public void AddRange(IEnumerable<Dir> items)
            {
                items.ToList().ForEach(Add);
            }

            public void Recreate(List<Dir> dirs)
            {
                CollectionChanged -= RecalcItems;
                Clear();
                AddRange(dirs);
                CollectionChanged += RecalcItems;
                RecalcItems();
            }

            private void NotifyItemChanged([CallerMemberName] string propertyName = "")
            {
                ItemChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public event PropertyChangedEventHandler ItemChanged;
        }

        public static readonly DependencyProperty FirstDirectoryProperty =
            DependencyProperty.Register(nameof(FirstDirectory), typeof(string), typeof(MultiFolder),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FirstDirectoryPropertyChangedCallback));

        private static void FirstDirectoryPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            (d as MultiFolder).OnFirstDirectoryPropertyChanged();
        }

        private void OnFirstDirectoryPropertyChanged()
        {
            _directories.First().Path = FirstDirectory;
        }

        public string FirstDirectory
        {
            get
            {
                return (string)GetValue(FirstDirectoryProperty);
            }
            set
            {
                if (value != (string)GetValue(FirstDirectoryProperty))
                {
                    SetValue(FirstDirectoryProperty, value);
                }

            }
        }


        public static readonly DependencyProperty DirectoriesProperty =
           DependencyProperty.Register(nameof(Directories), typeof(List<string>), typeof(MultiFolder),
               new FrameworkPropertyMetadata(new List<string>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DirectoriesPropertyChangedCallback));

        private static void DirectoriesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            (d as MultiFolder).OnDirectoriesPropertyChanged();
        }

        private void OnDirectoriesPropertyChanged()
        {
            _directories.CollectionChanged -= UpdateDirectories;
            _directories.Recreate(Directories.Select(path => new Dir(path)).ToList());
            _directories.CollectionChanged += UpdateDirectories;
        }

        private void UpdateDirectories(object sender, NotifyCollectionChangedEventArgs e)
        {
            Directories = _directories.Select(x => x.Path).ToList();
        }

        public List<string> Directories
        {
            get
            {
                return (List<string>)GetValue(DirectoriesProperty);
            }
            set
            {
                if (value != (List<string>)GetValue(DirectoriesProperty))
                {
                    SetValue(DirectoriesProperty, value);
                }

            }
        }

        private readonly DirObservableCollection _directories = new DirObservableCollection(new List<Dir> { new Dir() });

        public MultiFolder()
        {
            InitializeComponent();

            _directories.CollectionChanged += UpdateDirectories;
            _directories.ItemChanged += (o, e) =>
            {
                if (e.PropertyName == "FirstItem")
                {
                    FirstDirectory = _directories.FirstOrDefault().Path;
                }
            };

            MultiFolderItemsControl.ItemsSource = _directories;
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
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

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string path;
            var button = sender as Button;

            var dir = button.DataContext as Dir;
            path = dir.Path;
            if (!string.IsNullOrEmpty(path))
            {
                _directories.Add(new Dir());
            }
        }

        private void RemoveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dir = button.DataContext as Dir;

            if (_directories.Count > 1)
            {
                _directories.Remove(dir);
            }
        }

    }

}
