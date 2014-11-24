/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Windows.Forms;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of OutputViewControl.
	/// </summary>
	public class OutputViewControl : UserControl
	{
		private Button _cmdResetDatabase;
		private Button _cmdClear;
		private TextBox _console;
		private MainForm _main;
		
		public OutputViewControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			UIStyle.ApplyConsoleStyle(_console);
			UIStyle.ApplyButtonStyle(_cmdResetDatabase);
			UIStyle.ApplyButtonStyle(_cmdClear);
		}
		
		public MainForm MainForm
		{
			get
			{
				return _main;
			}
			
			set
			{
				_main = value;
			}
		}
		
		public void AppendText(string text)
		{
			_console.AppendText(text);
		}
		
		public void WriteLine(string text)
		{
			_console.AppendText(text + Environment.NewLine);
		}
		
		public void Clear()
		{
			_console.Text = "";
		}

		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this._console = new System.Windows.Forms.TextBox();
			this._cmdClear = new System.Windows.Forms.Button();
			this._cmdResetDatabase = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _console
			// 
			this._console.Cursor = System.Windows.Forms.Cursors.Default;
			this._console.Dock = System.Windows.Forms.DockStyle.Fill;
			this._console.Location = new System.Drawing.Point(0, 0);
			this._console.Multiline = true;
			this._console.Name = "_console";
			this._console.ReadOnly = true;
			this._console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._console.Size = new System.Drawing.Size(456, 312);
			this._console.TabIndex = 0;
			this._console.Text = "";
			// 
			// _cmdClear
			// 
			this._cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cmdClear.Location = new System.Drawing.Point(352, 8);
			this._cmdClear.Name = "_cmdClear";
			this._cmdClear.TabIndex = 1;
			this._cmdClear.Text = "Clear";
			this._cmdClear.Click += new System.EventHandler(this._cmdClearClick);
			// 
			// _cmdResetDatabase
			// 
			this._cmdResetDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cmdResetDatabase.Location = new System.Drawing.Point(248, 8);
			this._cmdResetDatabase.Name = "_cmdResetDatabase";
			this._cmdResetDatabase.Size = new System.Drawing.Size(96, 23);
			this._cmdResetDatabase.TabIndex = 2;
			this._cmdResetDatabase.Text = "Reset Database";
			this._cmdResetDatabase.Click += new System.EventHandler(this._cmdResetDatabaseClick);
			// 
			// OutputViewControl
			// 
			this.Controls.Add(this._cmdResetDatabase);
			this.Controls.Add(this._cmdClear);
			this.Controls.Add(this._console);
			this.Name = "OutputViewControl";
			this.Size = new System.Drawing.Size(456, 312);
			this.ResumeLayout(false);
		}
		#endregion
		void _cmdClearClick(object sender, System.EventArgs e)
		{
			Clear();
		}
		
		void _cmdResetDatabaseClick(object sender, System.EventArgs e)
		{
			_main.ResetDatabase();
		}
		
	}
}
