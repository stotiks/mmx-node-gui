using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
    /// <summary>
    /// Interaction logic for MultiDir.xaml
    /// </summary>
    public partial class MultiFolder
    {

        private const string FirstItem = "FirstItem";

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderTextProperty), typeof(string), typeof(MultiFolder), new PropertyMetadata(string.Empty));

        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                if (value != (string)GetValue(HeaderTextProperty))
                {
                    SetValue(HeaderTextProperty, value);
                }
            }
        }

        public static readonly DependencyProperty FirstDirectoryProperty =
            DependencyProperty.Register(nameof(FirstDirectory), typeof(string), typeof(MultiFolder),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FirstDirectoryPropertyChangedCallback));

        private static void FirstDirectoryPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            (d as MultiFolder).OnFirstDirectoryPropertyChanged();
        }

        private void OnFirstDirectoryPropertyChanged()
        {
            if (_directories.Count > 0)
            {
                _directories.First().Path = FirstDirectory;
            } else
            {
                _directories.Add(new Dir(FirstDirectory));
            }
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
           DependencyProperty.Register(nameof(Directories), typeof(ArrayList), typeof(MultiFolder),
               new FrameworkPropertyMetadata(new ArrayList() { "" }, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DirectoriesPropertyChangedCallback));

        private static void DirectoriesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            (d as MultiFolder).OnDirectoriesPropertyChanged();
        }

        private void OnDirectoriesPropertyChanged()
        {
            if (Directories != null)
            {
                if(Directories.Count == 0) 
                {
                    Directories.Add("");
                }

                _directories.DirCollectionChanged -= UpdateDirectories;
                _directories.Recreate(Directories.ToArray().Select(path => new Dir(path as string)).ToList());
                _directories.DirCollectionChanged += UpdateDirectories;
            }
        }

        private void UpdateDirectories()
        {
            Directories = new ArrayList(_directories.Select(x => x.Path).ToList());
        }

        public ArrayList Directories
        {
            get
            {
                return (ArrayList)GetValue(DirectoriesProperty);
            }
            set
            {
                if (value != (ArrayList)GetValue(DirectoriesProperty))
                {
                    SetValue(DirectoriesProperty, value);
                }
            }
        }

        private readonly DirObservableCollection _directories = new DirObservableCollection(new List<Dir> { new Dir() });

        public MultiFolder()
        {
            InitializeComponent();

            _directories.DirCollectionChanged += UpdateDirectories;
            _directories.ItemChanged += (o, e) =>
            {
                if (e.PropertyName == FirstItem)
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
