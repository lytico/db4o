using System;
using System.Threading;
using System.Windows.Forms;
using Timer=System.Threading.Timer;

namespace Db4objects.Db4o.SilverlightTestHost
{
	public partial class WebBrowerHost : Form
	{
		private string _url;
		private Timer _timer;

		public WebBrowerHost()
		{
			InitializeComponent();
		}

		public int ErrorCount { get; set; }

		public int Navigate(string url)
		{
			_url = url;
			ShowDialog();

			return ErrorCount;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			silverlightTestHost.Navigate(_url);
			_timer = new Timer(state => Invoke((ThreadStart) CheckTestsCompletion), null, 1000, 1000);
		}

		private void CheckTestsCompletion()
		{
			HtmlElement element = silverlightTestHost.Document.GetElementById("completed");
			if (element != null)
			{
				_timer.Dispose();
				string testResults = silverlightTestHost.Document.GetElementById("result").InnerText;

				ErrorCount = Int32.Parse(ErrorCountFromHtmlPage());
				Console.Error.WriteLine(testResults);

				Close();
			}
		}

		private string ErrorCountFromHtmlPage()
		{
			return silverlightTestHost.Document.InvokeScript("getErrorCount").ToString();
		}
	}
}
