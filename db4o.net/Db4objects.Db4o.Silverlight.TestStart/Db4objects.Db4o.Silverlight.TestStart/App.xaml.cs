/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;

namespace Db4objects.Db4o.Silverlight.TestStart
{
	public partial class App
	{
		public App()
		{
			Startup += Application_Startup;
			UnhandledException += Application_UnhandledException;

			InitializeComponent();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			RootVisual = new MainPage();
		}

		private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (!Debugger.IsAttached)
			{
				MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled Exception", MessageBoxButton.OK);

				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
			}
		}
		private static void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

				HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
			}
			catch (Exception)
			{
			}
		}
	}
}
