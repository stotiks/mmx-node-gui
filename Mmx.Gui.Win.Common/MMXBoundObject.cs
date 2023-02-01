using Mmx.Gui.Win.Common.Properties;

namespace Mmx.Gui.Win.Common
{
    public abstract class MMXBoundObject
    {
        public string Locale => Settings.Default.LanguageCode;

        public abstract bool Theme_dark
        {
            get;
        }

        public delegate void CopyKeysToPlotterEventHandler(string json);

        public event CopyKeysToPlotterEventHandler KeysToPlotter;

        public void CopyKeysToPlotter(string json)
        {
            KeysToPlotter?.Invoke(json);
        }

    }


}
