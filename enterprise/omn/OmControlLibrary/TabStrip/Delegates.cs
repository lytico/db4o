using System;

namespace OMControlLibrary
{
	#region TabStripItemClosingEventArgs

	public class TabStripItemClosingEventArgs : EventArgs
	{
		public TabStripItemClosingEventArgs(OMETabStripItem item)
		{
			_item = item;
		}

		private bool _cancel = false;
		private OMETabStripItem _item;

		public OMETabStripItem Item
		{
			get { return _item; }
			set { _item = value; }
		}

		public bool Cancel
		{
			get { return _cancel; }
			set { _cancel = value; }
		}

	}

	#endregion

	#region TabStripItemChangedEventArgs

	public class TabStripItemChangedEventArgs : EventArgs
	{
		OMETabStripItem itm;
		OMETabStripItemChangeTypes changeType;

		public TabStripItemChangedEventArgs(OMETabStripItem item, OMETabStripItemChangeTypes type)
		{
			changeType = type;
			itm = item;
		}

		public OMETabStripItemChangeTypes ChangeType
		{
			get { return changeType; }
		}

		public OMETabStripItem Item
		{
			get { return itm; }
		}
	}

	#endregion

	public delegate void TabStripItemChangedHandler(TabStripItemChangedEventArgs e);
	public delegate void TabStripItemClosingHandler(TabStripItemClosingEventArgs e);

}
