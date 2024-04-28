using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
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
                    if (_path != value) 
                    {
                        _path = value;
                        NotifyPropertyChanged();
                    }
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

            bool _isLastAndNotEmpty;
            public bool IsLastAndNotEmpty
            {
                get => _isLastAndNotEmpty;
                internal set
                {
                    _isLastAndNotEmpty = value;
                    NotifyPropertyChanged();
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }

    }

}
