using System;

namespace Mmx.Gui.Win.Common.Plotter
{

    [AttributeUsage(AttributeTargets.All)]
    public class UrlAttribute : Attribute
    {
        public string Url { get; private set; }

        public UrlAttribute(string url) => Url = url;
    }

}
