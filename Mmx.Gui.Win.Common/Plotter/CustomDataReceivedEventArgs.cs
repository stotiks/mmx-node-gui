using System;
using System.Diagnostics;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class CustomDataReceivedEventArgs : EventArgs
    {

        public string Data { get; set; }


        public CustomDataReceivedEventArgs(string data)
        {
            this.Data = data;
        }

        public CustomDataReceivedEventArgs(DataReceivedEventArgs source)
        {
            Data = source.Data;
        }
    }
}
