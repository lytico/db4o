using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class ToolTipComboBox : ComboBox
{
	private DropdownWindow mDropdown;
	public delegate void DropdownItemSelectedEventHandler(object sender, DropdownItemSelectedEventArgs e);
	public event DropdownItemSelectedEventHandler DropdownItemSelected;

	protected override void OnDropDown(EventArgs e)
	{
		try
		{
			// Install wrapper
			base.OnDropDown(e);
			// Retrieve handle to dropdown list
			COMBOBOXINFO info = new COMBOBOXINFO();
			info.cbSize = Marshal.SizeOf(info);
			SendMessageCb(this.Handle, 0x164, IntPtr.Zero, out info);
			mDropdown = new DropdownWindow(this);
			mDropdown.AssignHandle(info.hwndList);
		}
		catch { }
	}
	protected override void OnDropDownClosed(EventArgs e)
	{
		try
		{
			// Remove wrapper
			mDropdown.ReleaseHandle();
			mDropdown = null;
			base.OnDropDownClosed(e);
			OnSelect(-1, Rectangle.Empty, true);
		}
		catch { }
	}
	internal void OnSelect(int item, Rectangle pos, bool scroll)
	{
		try
		{
			if (this.DropdownItemSelected != null)
			{
				pos = this.RectangleToClient(pos);
				DropdownItemSelected(this, new DropdownItemSelectedEventArgs(item, pos, scroll));
			}
		}
		catch { }
	}
	// Event handler arguments
	public class DropdownItemSelectedEventArgs : EventArgs
	{
		private int mItem;
		private Rectangle mPos;
		private bool mScroll;
		public DropdownItemSelectedEventArgs(int item, Rectangle pos, bool scroll) { mItem = item; mPos = pos; mScroll = scroll; }
		public int SelectedItem { get { return mItem; } }
		public Rectangle Bounds { get { return mPos; } }
		public bool Scrolled { get { return mScroll; } }

	}

	// Wrapper for combobox dropdown list
	private class DropdownWindow : NativeWindow
	{
		private ToolTipComboBox mParent;
		private int mItem;
		public DropdownWindow(ToolTipComboBox parent)
		{
			mParent = parent;
			mItem = -1;
		}
		protected override void WndProc(ref Message m)
		{
			try
			{
				// All we're getting here is WM_MOUSEMOVE, ask list for current selection for LB_GETCURSEL
				Console.WriteLine(m.ToString());
				base.WndProc(ref m);
				if (m.Msg == 0x200)
				{
					int item = (int)SendMessage(this.Handle, 0x188, IntPtr.Zero, IntPtr.Zero);
					if (item != mItem)
					{
						mItem = item;
						OnSelect(false);
					}
				}
				if (m.Msg == 0x115)
				{
					// List scrolled, item position would change
					OnSelect(true);
				}
			}
			catch { }
		}
		private void OnSelect(bool scroll)
		{
			try
			{
				RECT rc = new RECT();
				SendMessageRc(this.Handle, 0x198, (IntPtr)mItem, out rc);
				MapWindowPoints(this.Handle, IntPtr.Zero, ref rc, 2);
				mParent.OnSelect(mItem, Rectangle.FromLTRB(rc.Left, rc.Top, rc.Right, rc.Bottom), scroll);
			}
			catch { }
		}
	}
	// P/Invoke declarations
	private struct COMBOBOXINFO
	{
		public Int32 cbSize;
		public RECT rcItem;
		public RECT rcButton;
		public int buttonState;
		public IntPtr hwndCombo;
		public IntPtr hwndEdit;
		public IntPtr hwndList;
	}
	[StructLayout(LayoutKind.Sequential)]
	private struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}
	[DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
	private static extern IntPtr SendMessageCb(IntPtr hWnd, int msg, IntPtr wp, out COMBOBOXINFO lp);
	[DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
	private static extern IntPtr SendMessageRc(IntPtr hWnd, int msg, IntPtr wp, out RECT lp);
	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
	[DllImport("user32.dll")]
	private static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref RECT rc, int points);
}