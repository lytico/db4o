/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of OutputView.
	/// </summary>
	public class OutputView : DockContent
	{
		OutputViewControl _console;
		
		public OutputView(MainForm main)
		{
			CloseButton = false;
			DockAreas = (
					DockAreas.Float |
					DockAreas.DockBottom |
					DockAreas.DockTop |
					DockAreas.DockLeft |
					DockAreas.DockRight);
			ShowHint = DockState.DockBottom;
			Text = "Output";
			
			_console = new OutputViewControl();
			_console.MainForm = main;
			_console.Dock = DockStyle.Fill;
			Controls.Add(_console);
		}
		
		public void AppendText(string text)
		{
			_console.AppendText(text);
		}
		
		public void WriteLine(string line)
		{
			_console.WriteLine(line);
		}
	}
}
