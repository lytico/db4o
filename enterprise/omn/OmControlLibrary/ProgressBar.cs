using System.Windows.Forms;

namespace OMControlLibrary
{
	public partial class ProgressBar : Form
	{
		public ProgressBar()
		{
			SetStyle(
				ControlStyles.CacheText | ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);

			InitializeComponent();
		}
	}
}