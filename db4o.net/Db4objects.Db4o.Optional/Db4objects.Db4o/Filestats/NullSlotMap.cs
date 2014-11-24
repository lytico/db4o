/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	public class NullSlotMap : ISlotMap
	{
		public virtual void Add(Slot slot)
		{
		}

		public virtual IList Merged()
		{
			return new ArrayList();
		}

		public virtual IList Gaps(long length)
		{
			return new ArrayList();
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
#endif // !SILVERLIGHT
