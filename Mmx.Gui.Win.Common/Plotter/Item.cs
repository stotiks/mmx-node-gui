using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public enum Plotters : int
    {
        MmxPlotter = 1 << 0,
        MmxPlotterCompressed = 1 << 1,
        MmxCudaPlotter = 1 << 2,
        MmxBladebit = 1 << 8,
    };

    public enum ItemType
    {
        CmdParameter,
        EnvParameter,
        Hidden,
        Other
    };

    public class PathItem : Item<string>
    {
        public new string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                var value = Value.ToString();

                if (value.Contains(" "))
                {
                    value = $"\"{value}\"";
                    value = value.Replace("\\\"", "\\\\\"");
                }

                if (!string.IsNullOrEmpty(value))
                {
                    result = FormatParam(value);
                }

            }

            return result;
        }
    }

    public class BoolItem : Item<bool>
    {
        public BoolItem()
        {
            SkipValue = true;
        }
        public new string GetParam()
        {
            return Value ? base.GetParam() : "";
        }
    }

    public class Item<T> : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string LongName { get; set; }

        private bool _valueInitialized;
        private T _value;
        public T Value
        {
            get => _value;
            set {
                _value = value;
                _valueInitialized = true;
                NotifyPropertyChanged();
            }
        }

        public void SetValue(object obj)
        {
            Value = (T)Convert.ChangeType(obj, typeof(T));
        }

        private T _defaultValue;
        public T DefaultValue
        {  
            get => _defaultValue;
            internal set {
                _defaultValue = value;
                if (_valueInitialized == false)
                {
                    _value = _defaultValue;
                }
                NotifyPropertyChanged();
            }
        }

        private bool _isVisible;
        public bool IsVisible { 
            get => _isVisible;
            internal set
            {
                _isVisible = value;
                NotifyPropertyChanged();
            }
        }

        public ItemType Type { get; internal set; } = ItemType.CmdParameter;
        //public bool IsPlotterParam { get; internal set; } = true;
        public PlotterOptions.Scopes Scope { get; internal set; } = PlotterOptions.Scopes.None;

        public ObservableCollection<Item<T>> Items { get; internal set; }
        public T Minimum { get; internal set; }
        public T Maximum { get; internal set; }
        public bool SkipName { get; internal set; }
        public bool SkipValue { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                var value = Value.ToString();
                if (!string.IsNullOrEmpty(value))
                {                    
                    result = FormatParam(value);
                }
            }

            return result;
        }

        protected string FormatParam(string value)
        {
            string result;

            if (SkipName)
            {
                result = value;
            } else
            {
                if(SkipValue)
                {
                    result = $"-{Name}";
                } else
                {
                    result = $"-{Name} {value}";
                }                
            }

            return result;
        }

    }

}
