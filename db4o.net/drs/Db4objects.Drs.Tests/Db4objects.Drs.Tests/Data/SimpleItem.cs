/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class SimpleItem
	{
		private string value;

		private Db4objects.Drs.Tests.Data.SimpleItem child;

		private SimpleListHolder parent;

		public SimpleItem()
		{
		}

		public SimpleItem(SimpleListHolder parent_, Db4objects.Drs.Tests.Data.SimpleItem 
			child_, string value_)
		{
			parent = parent_;
			value = value_;
			child = child_;
		}

		public SimpleItem(string value_) : this(null, null, value_)
		{
		}

		public SimpleItem(Db4objects.Drs.Tests.Data.SimpleItem child, string value_) : this
			(null, child, value_)
		{
		}

		public SimpleItem(SimpleListHolder parent_, string value_) : this(parent_, null, 
			value_)
		{
		}

		public virtual string GetValue()
		{
			return value;
		}

		public virtual void SetValue(string value_)
		{
			value = value_;
		}

		public virtual Db4objects.Drs.Tests.Data.SimpleItem GetChild()
		{
			return GetChild(0);
		}

		public virtual Db4objects.Drs.Tests.Data.SimpleItem GetChild(int level)
		{
			Db4objects.Drs.Tests.Data.SimpleItem tbr = child;
			while (--level > 0 && tbr != null)
			{
				tbr = tbr.child;
			}
			return tbr;
		}

		public virtual void SetChild(Db4objects.Drs.Tests.Data.SimpleItem child_)
		{
			child = child_;
		}

		public virtual SimpleListHolder GetParent()
		{
			return parent;
		}

		public virtual void SetParent(SimpleListHolder parent_)
		{
			parent = parent_;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Db4objects.Drs.Tests.Data.SimpleItem))
			{
				return false;
			}
			Db4objects.Drs.Tests.Data.SimpleItem rhs = (Db4objects.Drs.Tests.Data.SimpleItem)
				obj;
			return rhs.GetValue().Equals(GetValue());
		}

		public override string ToString()
		{
			string childString;
			if (child != null)
			{
				childString = child != this ? child.ToString() : "this";
			}
			else
			{
				childString = "null";
			}
			return value + "[" + childString + "]";
		}
	}
}
