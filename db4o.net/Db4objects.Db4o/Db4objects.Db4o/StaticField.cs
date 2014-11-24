/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <exclude></exclude>
	/// <persistent></persistent>
	public class StaticField : IInternal4
	{
		public string name;

		public object value;

		public StaticField()
		{
		}

		public StaticField(string name_, object value_)
		{
			name = name_;
			value = value_;
		}
	}
}
