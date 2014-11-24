using System;
using System.IO;

namespace Db4objects.Db4o.SilverlightTestHost
{
	class Program
	{
		[STAThread]
		static int Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.Error.WriteLine("Missing silverlight tests folder.");
				return -1;
			}

			WebBrowerHost host = new WebBrowerHost();
			return host.Navigate(Path.Combine(args[0], "TestPage.html"));
		}
	}
}
