using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class ItemBase<T>: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool _valueInitialized;
        protected T _value;

        public string Name { get; set; }
        public T Value
        {
            get => _value;
            set
            {
                _valueInitialized = true;
                if (!_value.Equals(value))
                {
                    _value = value;
                    NotifyPropertyChanged();
                }
                
            }
        }

        public virtual JToken JValue => new JValue(Value);
    }

}
