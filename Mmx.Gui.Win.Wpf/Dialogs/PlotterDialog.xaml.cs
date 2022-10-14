using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Mmx.Gui.Win.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for PlotterDialog.xaml
    /// </summary>
    public partial class PlotterDialog : INotifyPropertyChanged
    {
        public PlotterDialog()
        {
            InitializeComponent();
            DataContext = this;

            PropertyChanged += (s, e) => 
            {
                switch (e.PropertyName)
                {
                    case "ProcessSuspended":
                        if (_processSuspended)
                        {
                            PauseButton.Content = Properties.Resources.Plotter_Resume;
                        }
                        else
                        {
                            PauseButton.Content = Properties.Resources.Plotter_Pause;
                        }
                        break;
                }
            };
        }

        private string _logTxt;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LogTxt
        {
            get => _logTxt;
            set
            {
                _logTxt = value;
                NotifyPropertyChanged();
            }
        }

        private bool _processSuspended = false;
        public bool ProcessSuspended
        {
            get => _processSuspended;

            set
            {
                _processSuspended = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("FullDialogSizing") is VisualState fullDialogSizing)
            {
                if (fullDialogSizing.Storyboard != null)
                {
                    var anim = new ObjectAnimationUsingKeyFrames
                    {
                        KeyFrames = { new DiscreteObjectKeyFrame(HorizontalAlignment.Stretch, TimeSpan.Zero) }
                    };
                    Storyboard.SetTargetName(anim, "BackgroundElement");
                    Storyboard.SetTargetProperty(anim, new PropertyPath(HorizontalAlignmentProperty));
                    fullDialogSizing.Storyboard.Children.Add(anim);
                }
            }
            
            base.OnApplyTemplate();
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxLog.ScrollToEnd();
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                args.Cancel = true;
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
