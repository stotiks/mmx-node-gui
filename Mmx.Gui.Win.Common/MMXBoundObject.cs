namespace Mmx.Gui.Win.Common
{
    public class MMXBoundObject
    {
        public string Locale 
        {
            get => Properties.Settings.Default.LanguageCode;
        }
        public bool Theme_dark
        {
            get => Properties.Settings.IsDarkTheme;
        }

        public delegate void CopyKeysToPlotterEventHandler(string json);

        public event CopyKeysToPlotterEventHandler KeysToPlotter;

        public void CopyKeysToPlotter(string json)
        {
            KeysToPlotter?.Invoke(json);
        }

    }


}
