using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using stdole;


namespace OMControlLibrary.Common
{

	public class PictureHost : System.Windows.Forms.AxHost
	{


		public PictureHost()
			: base("59EE46BA-677D-4d20-BF10-8D8067CB8B33")
		{
		}

		public static IPictureDisp IPictureDisp(System.Drawing.Image Image)
		{
			return (IPictureDisp)AxHost.GetIPictureDispFromPicture(Image);
		}

	}
}
