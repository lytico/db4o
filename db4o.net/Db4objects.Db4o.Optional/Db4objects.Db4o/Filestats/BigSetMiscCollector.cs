/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	internal class BigSetMiscCollector : IMiscCollector
	{
		public virtual long CollectFor(LocalObjectContainer db, int id, ISlotMap slotMap)
		{
			object bigSet = db.GetByID(id);
			db.Activate(bigSet, 1);
			BTree btree = (BTree)Reflection4.GetFieldValue(bigSet, "_bTree");
			return FileUsageStatsCollector.BTreeUsage(db, btree, slotMap);
		}
	}
}
#endif // !SILVERLIGHT
