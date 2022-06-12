using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public class MMXBoundObject
    {
        MainForm mainForm;

        public MMXBoundObject(MainForm form)
        {
            mainForm = form;
        }

        public string Locale
        {
            get
            {
                return Properties.Settings.Default.langCode;
            }
        }

        public void PostMessage(String message)
        {
            MessageBox.Show("Message from webpage via postMessage API:\n" + message, "Hosting .NET Application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void CopyKeysToPlotter(string json)
        {
            mainForm.CopyKeysToPlotter(json);
        }

    }
}
