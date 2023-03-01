using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public enum ItemType
    {
        CmdParameter,
        EnvParameter,
        Hidden,
        Other
    };

    public class IntItem : Item<int> {}
    public class StringItem : Item<string> { }

    public class PathItem : Item<string>
    {
        protected override string FormatValue(string value)
        {
            if (value.Contains(" "))
            {
                value = $"\"{value}\"";
                value = value.Replace("\\\"", "\\\\\"");
            }

            return value;
        }
    }

    public class BoolItem : Item<bool>
    {
        public BoolItem()
        {
            SkipValue = true;
        }
        public override string GetParam()
        {
            return Value ? base.GetParam() : "";
        }
    }

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

    public abstract class Item<T> : ItemBase<T>
    {
        public string LongName { get; internal set; }

        public virtual void SetValue(object obj)
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

        private bool _skip;
        public bool Skip
        {
            get => _skip;
            internal set
            {
                _skip = value;
                NotifyPropertyChanged();
            }
        }

        public ItemType Type { get; internal set; } = ItemType.CmdParameter;

        public PlotterOptions.Scopes Scope { get; internal set; } = PlotterOptions.Scopes.None;

        public ObservableCollection<ItemBase<T>> Items { get; internal set; }
        public T Minimum { get; internal set; }
        public T Maximum { get; internal set; }
        public bool SkipName { get; internal set; }
        public bool SkipValue { get; internal set; }
        public bool Persistent { get; internal set; } = true;
        public bool SuppressDefaultValue { get; internal set; } = false;        

        public virtual string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                var value = Value.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    value = FormatValue(value);
                    result = FormatParam(value);
                }
            }

            return result;
        }

        protected virtual string FormatValue(string value)
        {
            return value;
        }

        protected string FormatParam(string value)
        {
            string result = "";

            if(Skip || SuppressDefaultValue && value == DefaultValue.ToString())
            {
                return result;
            }

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
