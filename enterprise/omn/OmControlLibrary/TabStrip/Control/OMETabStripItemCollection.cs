using System;
using System.ComponentModel;
using OMControlLibrary.Helpers;

namespace OMControlLibrary
{
	public class OMETabStripItemCollection : CollectionWithEvents
	{
		#region Fields

		[Browsable(false)]
		public event CollectionChangeEventHandler CollectionChanged;

		private int lockUpdate;

		#endregion

		#region Ctor

		public OMETabStripItemCollection()
		{
			lockUpdate = 0;
		}

		#endregion

		#region Props

		public OMETabStripItem this[int index]
		{
			get
			{
				if (index < 0 || List.Count - 1 < index)
					return null;

				return (OMETabStripItem)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		[Browsable(false)]
		public virtual int DrawnCount
		{
			get
			{
				int count = Count, res = 0;
				if (count == 0) return 0;
				for (int n = 0; n < count; n++)
				{
					if (this[n].IsDrawn)
						res++;
				}
				return res;
			}
		}

		public virtual OMETabStripItem LastVisible
		{
			get
			{
				for (int n = Count - 1; n > 0; n--)
				{
					if (this[n].Visible)
						return this[n];
				}

				return null;
			}
		}

		public virtual OMETabStripItem FirstVisible
		{
			get
			{
				for (int n = 0; n < Count; n++)
				{
					if (this[n].Visible)
						return this[n];
				}

				return null;
			}
		}

		[Browsable(false)]
		public virtual int VisibleCount
		{
			get
			{
				int count = Count, res = 0;
				if (count == 0) return 0;
				for (int n = 0; n < count; n++)
				{
					if (this[n].Visible)
						res++;
				}
				return res;
			}
		}

		#endregion

		#region Methods

		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (CollectionChanged != null)
				CollectionChanged(this, e);
		}

		protected virtual void BeginUpdate()
		{
			lockUpdate++;
		}

		protected virtual void EndUpdate()
		{
			if (--lockUpdate == 0)
				OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		public virtual void AddRange(OMETabStripItem[] items)
		{
			BeginUpdate();
			try
			{
				foreach (OMETabStripItem item in items)
				{
					List.Add(item);
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		public virtual void Assign(OMETabStripItemCollection collection)
		{
			BeginUpdate();
			try
			{
				Clear();
				for (int n = 0; n < collection.Count; n++)
				{
					OMETabStripItem item = collection[n];
					OMETabStripItem newItem = new OMETabStripItem();
					newItem.Assign(item);
					Add(newItem);
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		public virtual int Add(OMETabStripItem item)
		{
			int res = IndexOf(item);
			if (res == -1) res = List.Add(item);
			return res;
		}

		public virtual void Remove(OMETabStripItem item)
		{
			if (List.Contains(item))
				List.Remove(item);
		}

		public virtual OMETabStripItem MoveTo(int newIndex, OMETabStripItem item)
		{
			int currentIndex = List.IndexOf(item);
			if (currentIndex >= 0)
			{
				RemoveAt(currentIndex);
				Insert(0, item);

				return item;
			}

			return null;
		}

		public virtual int IndexOf(OMETabStripItem item)
		{
			return List.IndexOf(item);
		}

		public virtual bool Contains(OMETabStripItem item)
		{
			return List.Contains(item);
		}

		public virtual void Insert(int index, OMETabStripItem item)
		{
			if (Contains(item)) return;
			List.Insert(index, item);
		}

		protected override void OnInsertComplete(int index, object item)
		{
			OMETabStripItem itm = item as OMETabStripItem;
			itm.Changed += new EventHandler(OnItem_Changed);
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
		}

		protected override void OnRemove(int index, object item)
		{
			base.OnRemove(index, item);
			OMETabStripItem itm = item as OMETabStripItem;
			itm.Changed -= new EventHandler(OnItem_Changed);
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
		}

		protected override void OnClear()
		{
			if (Count == 0) return;
			BeginUpdate();
			try
			{
				for (int n = Count - 1; n >= 0; n--)
				{
					RemoveAt(n);
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		protected virtual void OnItem_Changed(object sender, EventArgs e)
		{
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, sender));
		}

		#endregion
	}
}
