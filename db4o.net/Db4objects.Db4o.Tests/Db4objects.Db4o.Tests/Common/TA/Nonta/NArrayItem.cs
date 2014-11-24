/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	public class NArrayItem
	{
		public int[][] value;

		public object obj;

		public LinkedList[][] lists;

		public object listsObject;

		public NArrayItem()
		{
		}

		public virtual int[][] Value()
		{
			return value;
		}

		public virtual object Object()
		{
			return obj;
		}

		public virtual LinkedList[][] Lists()
		{
			return lists;
		}

		public virtual object ListsObject()
		{
			return listsObject;
		}
	}
}
