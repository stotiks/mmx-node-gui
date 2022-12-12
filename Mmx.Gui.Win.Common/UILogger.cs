using Mmx.Gui.Win.Common.Plotter;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common
{

    public class UILogger: INotifyPropertyChanged
    {
        const int limit = 1000;
        const int notifyTextChangedDebounceTime  = 100;

        private readonly object _logLock = new object();
        private readonly Queue logQueue = new Queue();
      
        public string Text => string.Join("\r\n", logQueue.ToArray());

        private Action DebounceNotifyTextChanged;
        public UILogger() 
        {
            DebounceNotifyTextChanged = ((Action)NotifyTextChanged).Debounce(notifyTextChangedDebounceTime);
        }

        public void OutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            Write(args.Data);
        }

        public void Write(string log)
        {
            lock (_logLock)
            {
                logQueue.Enqueue(log);
                if (logQueue.Count > limit)
                {
                    logQueue.Dequeue();
                }

                DebounceNotifyTextChanged();
            }
        }

        public void Clear()
        {
            logQueue.Clear();
            NotifyTextChanged();
        }

        public void NotifyTextChanged()
        {
            NotifyPropertyChanged(nameof(Text));
        }

        public void ErrorDataReceived(object sender, DataReceivedEventArgs args)
        {
            OutputDataReceived(sender, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
