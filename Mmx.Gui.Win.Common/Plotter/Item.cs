﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;

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

    public class MultiPathItem : Item<ArrayList>
    {
        public override JToken JValue => new JArray(Value);

        public override void SetValue(object obj)
        {
            Value = (obj as JArray).ToObject<ArrayList>();
        }

        public override string GetParam()
        {
            var result = "";

            if (Value != null && Value.Count > 0)
            {
                for (int i = 0; i < Value.Count; i++)
                {
                    if (Value[i] != null)
                    {
                        var value = Value[i].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = FormatValue(value);
                            result += (i == 0? "" : " ") + FormatParam(value);
                        }
                    }
                }
            }

            return result;
        }

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

    public abstract class Item<T> : ItemBase<T>
    {
        public string LongName { get; internal set; }
        public bool UseCmdLongName { get; internal set; } = false;

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

        private ObservableCollection<ItemBase<T>> _items;
        public ObservableCollection<ItemBase<T>> Items 
        {
            get => _items;
            internal set
            {
                _items = value;
                NotifyPropertyChanged(); //notify [Items] property changed
                NotifyPropertyChanged(nameof(Value)); //notify [Value] property changed
            }
        }
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
                var name = $"-{Name}";

                if(UseCmdLongName)
                {
                    name = $"--{LongName}";
                }

                if(SkipValue)
                {
                    result = name;
                } else
                {
                    result = $"{name} {value}";
                }                
            }

            return result;
        }

    }

}
