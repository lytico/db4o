/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <exclude></exclude>
	public class Entry : ICompare, IInternal4
	{
		public object key;

		public object value;

		public virtual object Compare()
		{
			return key;
		}
	}
}
