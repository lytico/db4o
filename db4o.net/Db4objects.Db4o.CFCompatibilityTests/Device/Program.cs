using System;
using System.Windows.Forms;

namespace Db4objects.Db4o.CFCompatibilityTests.Device
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static int Main(string[] args)
		{
			Console console = new Console(args);
			Application.Run(console);

			return console.Result;
		}
	}
}