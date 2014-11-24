/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using EnvDTE;
using EnvDTE80;

namespace OMAddin
{
	class OutputWindow
	{
		public static void Initialize(DTE2 applicationObject)
		{
			if (_instance == null)
			{
				_instance = new OutputWindow(applicationObject);
			}
		}

		public static OutputWindowPane Pane
		{
			get
			{
				return _instance._outputWindowPane;
			}
		}

		private OutputWindow(DTE2 applicationObject)
		{
			if (null == _outputWindowPane)
			{
				EnvDTE.OutputWindow outputWindow = (EnvDTE.OutputWindow)applicationObject.Windows.Item(Constants.vsWindowKindOutput).Object;
				_outputWindowPane = outputWindow.OutputWindowPanes.Add("OMN Output");
			}
		}

		private readonly OutputWindowPane _outputWindowPane;
		private static OutputWindow _instance;
	}
}
