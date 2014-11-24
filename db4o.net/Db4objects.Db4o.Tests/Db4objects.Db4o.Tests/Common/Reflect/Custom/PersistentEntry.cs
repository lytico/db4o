/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class PersistentEntry
	{
		public string className;

		public object uid;

		public object[] fieldValues;

		public PersistentEntry()
		{
		}

		public PersistentEntry(string className, object uid, object[] fieldValues)
		{
			this.className = className;
			this.uid = uid;
			this.fieldValues = fieldValues;
		}

		public override string ToString()
		{
			return "PersistentEntry(" + className + ", " + uid + ", " + new Collection4(fieldValues
				) + ")";
		}
	}
}
