namespace Mmx.Gui.Win.Common
{
    public class MMXBoundObject
    {
        public string Locale => Properties.Settings.Default.LanguageCode;

        public bool Theme_dark => Properties.Settings.IsDarkTheme;

        public delegate void CopyKeysToPlotterEventHandler(string json);

        public event CopyKeysToPlotterEventHandler KeysToPlotter;

        public void CopyKeysToPlotter(string json)
        {
            KeysToPlotter?.Invoke(json);
        }

    }


}
