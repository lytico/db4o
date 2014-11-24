/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class HashtableLongEntry : HashtableIntEntry
	{
		public long _longKey;

		internal HashtableLongEntry(long key, object obj) : base((int)key, obj)
		{
			// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1
			_longKey = key;
		}

		public HashtableLongEntry() : base()
		{
		}

		public override object Key()
		{
			return _longKey;
		}

		public override object DeepClone(object obj)
		{
			return DeepCloneInternal(new Db4objects.Db4o.Foundation.HashtableLongEntry(), obj
				);
		}

		protected override HashtableIntEntry DeepCloneInternal(HashtableIntEntry entry, object
			 obj)
		{
			((Db4objects.Db4o.Foundation.HashtableLongEntry)entry)._longKey = _longKey;
			return base.DeepCloneInternal(entry, obj);
		}

		public override bool SameKeyAs(HashtableIntEntry other)
		{
			return other is Db4objects.Db4o.Foundation.HashtableLongEntry ? ((Db4objects.Db4o.Foundation.HashtableLongEntry
				)other)._longKey == _longKey : false;
		}

		public override string ToString()
		{
			return string.Empty + _longKey + ": " + _object;
		}
	}
}
