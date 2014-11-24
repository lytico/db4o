using System;
using System.Windows.Forms;
using OMControlLibrary.Common;
using OME.Logging.Common;

namespace OMControlLibrary
{
    public partial class AboutOME : Form
    {
        public AboutOME(string db4oVersion)
        {
            InitializeComponent();
        	labeldb4o.Text = "db4o (" + db4oVersion + ")";
        }

    	private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}