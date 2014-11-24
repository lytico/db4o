/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

using System.Drawing;
using System.Windows.Forms;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of UIStyle.
	/// </summary>
	public class UIStyle
	{
        public static readonly Color Db4oGrey = Color.FromArgb(0xFF, 255, 255, 255);

        public static readonly Color Db4oRed = Color.FromArgb(0xFF, 187, 30, 30);
    
		public static readonly Color BackColor = Color.FromArgb(0xFF, 0x83, 0x83, 0x83);
		
		public static readonly Color TextColor = Color.White;
		
		public static void Apply(Control control)
		{
			control.BackColor = UIStyle.BackColor;
			control.ForeColor = UIStyle.TextColor;
		}
		
		public static void ApplyConsoleStyle(Control control)
		{
			control.BackColor = UIStyle.BackColor;
            control.ForeColor = Color.Black;
		}
		
		public static void ApplyButtonStyle(Control control)
		{
			control.ForeColor = UIStyle.Db4oGrey;
			control.BackColor = UIStyle.Db4oRed;
		}
		
		private UIStyle()
		{
		}
	}
}
