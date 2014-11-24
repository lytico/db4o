/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class TreeStringObject : TreeString
	{
		public readonly object _value;

		public TreeStringObject(string key, object value) : base(key)
		{
			this._value = value;
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Foundation.TreeStringObject tso = new Db4objects.Db4o.Foundation.TreeStringObject
				(_key, _value);
			return ShallowCloneInternal(tso);
		}
	}
}
