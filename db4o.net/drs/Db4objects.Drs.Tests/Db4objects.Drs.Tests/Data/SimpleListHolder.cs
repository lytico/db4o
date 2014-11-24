/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class SimpleListHolder
	{
		private string _name;

		private IList list = new ArrayList();

		public SimpleListHolder()
		{
		}

		public SimpleListHolder(string name)
		{
			_name = name;
		}

		public virtual IList GetList()
		{
			return list;
		}

		public virtual void SetList(IList list)
		{
			this.list = list;
		}

		public virtual void Add(SimpleItem item)
		{
			list.Add(item);
		}

		public virtual void SetName(string name)
		{
			_name = name;
		}

		public virtual string GetName()
		{
			return _name;
		}
	}
}
