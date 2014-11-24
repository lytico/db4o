/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <exclude></exclude>
	/// <persistent></persistent>
	public class StaticClass : IInternal4
	{
		public string name;

		public StaticField[] fields;

		public StaticClass()
		{
		}

		public StaticClass(string name_, StaticField[] fields_)
		{
			name = name_;
			fields = fields_;
		}
	}
}
