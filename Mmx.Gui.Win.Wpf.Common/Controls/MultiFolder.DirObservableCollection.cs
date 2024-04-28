using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
    public partial class MultiFolder
    {
        class DirObservableCollection : ObservableCollection<Dir>, INotifyPropertyChanged
        {
            public DirObservableCollection(List<Dir> list) : base(list)
            {
                RecalcItems();
                this.ToList().ForEach(item => item.PropertyChanged += ItemPropertyChanged);
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
                if(index == 0)
                {
                    NotifyItemChanged(FirstItem);
                }    
            }

            private void RecalcItems(object sender, NotifyCollectionChangedEventArgs e)
            {
                RecalcItems();
                OnDirCollectionChanged();
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
                        NotifyItemChanged(FirstItem);
                    }
                    RecalcItems();
                    OnDirCollectionChanged();
                }
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

            private void OnDirCollectionChanged()
            {
                DirCollectionChanged?.Invoke();
            }

            public delegate void NotifyDirCollectionChangedEventHandler();
            public event NotifyDirCollectionChangedEventHandler DirCollectionChanged;
        }

    }

}
