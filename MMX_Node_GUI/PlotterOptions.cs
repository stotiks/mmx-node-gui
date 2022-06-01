using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MMX_NODE_GUI
{
    public class Item<T> : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }

        private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; NotifyPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                if (typeof(T) == typeof(bool))
                {
                    if ((bool)(object)Value == true)
                    {
                        result = string.Format("-{0}", Name); ;
                    }
                }
                else if (!string.IsNullOrEmpty(Value.ToString()))
                {
                    result = string.Format("-{0} {1}", Name, Value);
                }
            }

            return result;
        }

    }

    public class PlotterOptions : INotifyPropertyChanged
    {

        public Item<int> count = new Item<int>
        {
            Name = "n",
            LongName = "count",
            Value = -1,
            Description = "Number of plots to create"
        };

        public Item<int> size = new Item<int>
        {
            Name = "k",
            LongName = "size",
            Value = 32,
            Description = "K size"
        };

        public Item<int> port = new Item<int>
        {
            Name = "x",
            LongName = "port",
            Value = 11337,
            Description = "Network port"
        };

        public Item<int> threads = new Item<int>
        {
            Name = "r",
            LongName = "threads",
            Value = Environment.ProcessorCount,
            Description = "Number of threads"
        };

        public Item<int> buckets = new Item<int>
        {
            Name = "u",
            LongName = "buckets",
            Value = 256,
            Description = "Number of buckets"
        };

        public Item<int> buckets3 = new Item<int>
        {
            Name = "v",
            LongName = "buckets3",
            Value = 256,
            Description = "Number of buckets for phase 3+4"
        };

        public Item<int> rmulti2 = new Item<int>
        {
            Name = "K",
            LongName = "rmulti2",
            Value = 1,
            Description = "Thread multiplier for P2"
        };

        public Item<string> tmpdir = new Item<string>
        {
            Name = "t",
            LongName = "tmpdir",
            Value = "",
            Description = "Temporary directory, needs ~220 GiB"
        };

        public Item<string> tmpdir2 = new Item<string>
        {
            Name = "2",
            LongName = "tmpdir2",
            Value = "",
            Description = "Temporary directory 2, needs ~110 GiB [RAM]"
        };

        public Item<string> finaldir = new Item<string>
        {
            Name = "d",
            LongName = "finaldir",
            Value = "",
            Description = "Final directory to copy plot in parallel"
        };

        public Item<string> stagedir = new Item<string>
        {
            Name = "s",
            LongName = "stagedir",
            Value = "",
            Description = "Stage directory to write plot file"
        };


        public Item<string> poolkey = new Item<string>
        {
            Name = "p",
            LongName = "poolkey",
            Value = "",
            Description = "Pool Public Key (48 bytes)"
        };

        public Item<string> contract = new Item<string>
        {
            Name = "c",
            LongName = "contract",
            Value = "",
            Description = "Pool Contract Address (62 chars)"
        };

        public Item<string> farmerkey = new Item<string>
        {
            Name = "f",
            LongName = "farmerkey",
            Value = "",
            Description = "Farmer Public Key (48 bytes)"
        };

        public Item<bool> waitforcopy = new Item<bool>
        {
            Name = "w",
            LongName = "waitforcopy",
            Value = false,
            Description = "Wait for copy to start next plot"
        };

        public Item<bool> tmptoggle = new Item<bool>
        {
            Name = "G",
            LongName = "tmptoggle",
            Value = false,
            Description = "Alternate tmpdir/tmpdir2"
        };

        public Item<bool> directout = new Item<bool>
        {
            Name = "D",
            LongName = "directout",
            Value = false,
            Description = "Create plot directly in finaldir"
        };

        public Item<bool> nftplot = new Item<bool>
        {
            Name = "nftplot",
            LongName = "nftplot",
            Value = false,
            Description = "Create NFT plot"
        };

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("PlotterCmd"));
                Save();
            }
        }

        public PlotterOptions()
        {

            foreach (FieldInfo field in GetType().GetFields())
            {
                var fieldType = field.FieldType;

                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Item<>))
                {
                    (field.GetValue(this) as INotifyPropertyChanged).PropertyChanged += (sender, e) => NotifyPropertyChanged();
                }
            }

            string json = "{}";

            try
            {
                json = File.ReadAllText(Node.plotterConfigPath);               
            }
            catch
            {
                //System.Console.WriteLine(@"config not found");
            }

            LoadJSON(json);
        }

        internal void LoadJSON(string json)
        {
            dynamic config = JsonConvert.DeserializeObject(json);
            foreach (var configItem in config)
            {
                var field = typeof(PlotterOptions).GetField(configItem.Name);
                dynamic item = field.GetValue(this);
                item.Value = Convert.ChangeType(configItem.Value, field.FieldType.GenericTypeArguments[0]);
            }
        }

        private void Save()
        {
            dynamic jObject = new JObject();

            foreach (FieldInfo field in GetType().GetFields())
            {
                var fieldType = field.FieldType;
                var isItem = fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Item<>);

                if (isItem)
                {
                    dynamic item = field.GetValue(this);
                    jObject.Add(item.LongName, item.Value);
                }
            }

            var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            File.WriteAllText(Node.plotterConfigPath, json);

        }

        public string PlotterCmd
        {
            get
            {
                return string.Format("{0} {1}", PlotterExe, PlotterArguments);
            }
        }

        public string PlotterExe
        {
            get {
                var exe = "mmx_plot.exe";
                if (size.Value > 32)
                {
                    exe = "mmx_plot_k34.exe";
                }

                return exe;
            }
        }

        public string PlotterArguments
        {
            get
            {
                var result = "";
                foreach (FieldInfo field in GetType().GetFields())
                {
                    var fieldType = field.FieldType;
                    var isItem = fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Item<>);

                    if ( field.Name == "nftplot" ||
                         nftplot.Value && field.Name == "poolkey" ||
                         !nftplot.Value && field.Name == "contract"
                       )
                    {
                        continue;
                    }

                    if (isItem)
                    {
                        dynamic item = field.GetValue(this);
                        if (!string.IsNullOrEmpty(item.GetParam()))
                        {
                            result += result = string.Format(" {0}", item.GetParam());
                        }
                    }
                }

                return result;
            }
        }
    }


}
