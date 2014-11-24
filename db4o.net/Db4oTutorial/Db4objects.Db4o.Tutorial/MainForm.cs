/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : Form
	{
		readonly DockPanel _dockPanel;
		readonly OutputView _outputView;
		readonly TutorialOutlineView _outlineView;
		readonly WebBrowserView _webBrowserView;
		readonly ExampleRunner _exampleRunner;
		
		public MainForm()
		{
			InitializeComponent();
			
			_dockPanel = new DockPanel();
			_dockPanel.Dock = DockStyle.Fill;
			_dockPanel.DocumentStyle = DocumentStyle.DockingMdi;
			
			Controls.Add(_dockPanel);
			
			_outlineView = new TutorialOutlineView(this);
			_outputView = new OutputView(this);
			_webBrowserView = new WebBrowserView();			
			_webBrowserView.External = this;
			
			_exampleRunner = new ExampleRunner();
		}
		
		override protected void OnLoad(EventArgs args)
		{
			base.OnLoad(args);
			LoadViews();
		}
		
		public void NavigateTutorial(string href)
		{
			_webBrowserView.Navigate(GetTutorialFilePath(href));
		}
		
		public string GetTutorialFilePath(string fname)
		{
			return Path.Combine(GetTutorialBasePath(), fname);
		}
		
		public string GetTutorialBasePath()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs/");
		}

		public string CombineTutorialPath(string path)
		{
			return Path.Combine(GetTutorialBasePath(), path);
		}

		public void ResetDatabase()
		{
			_outputView.WriteLine("[FULL RESET]");
			_exampleRunner.Reset();
		}
		
		public void RunExample(string typeName, string method)
		{	
			InternalRunExample(ToNewConventions(typeName), ToPascalCase(method));
		}
		
		private string ToNewConventions(string typeName)
		{
			return ToPascalCaseNamespace(typeName.Replace("com.db4odoc", "Db4odoc.Tutorial"));
		}
		
		private string ToPascalCaseNamespace(string name)
		{
			StringBuilder builder = new StringBuilder();
			foreach (string part in name.Split('.'))
			{
				if (builder.Length > 0) builder.Append('.');
				builder.Append(ToPascalCase(part));
			}
			return builder.ToString();
		}
		
		private string ToPascalCase(string name)
		{
			return name.Length > 1
				? name.Substring(0, 1).ToUpper() + name.Substring(1)
				: name.ToUpper();
		}
		
		private void InternalRunExample(string typeName, string method)
		{
			_outputView.WriteLine("[" + method + "]");
			Cursor current = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				StringWriter output = new StringWriter();
				_exampleRunner.Run(typeName, method, output);
				_outputView.AppendText(output.ToString());
			}
			catch (Exception x)
			{
				_outputView.AppendText(x.ToString());
				Console.Error.WriteLine(x);
			}
			finally
			{
				Cursor.Current = current;
			}
			_outputView.WriteLine("");
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.SuspendLayout();
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "db4o tutorial";
			this.ResumeLayout(false);

		}
		#endregion
		
		void LoadViews()
		{
			_outputView.Show(_dockPanel);
			_outlineView.Show(_dockPanel);
			_webBrowserView.Show(_dockPanel);
		}
	}
}
