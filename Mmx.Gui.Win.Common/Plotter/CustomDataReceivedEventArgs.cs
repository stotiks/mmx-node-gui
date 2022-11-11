using System;
using System.Diagnostics;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class CustomDataReceivedEventArgs : EventArgs
    {

        public string Data { get; set; }


        public CustomDataReceivedEventArgs(string Data)
        {
            this.Data = Data;
        }

        public CustomDataReceivedEventArgs(DataReceivedEventArgs Source)
        {
            Data = Source.Data;
        }
    }
}
