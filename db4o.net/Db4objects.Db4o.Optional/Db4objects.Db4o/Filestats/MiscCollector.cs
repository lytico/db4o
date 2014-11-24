/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	internal interface IMiscCollector
	{
		long CollectFor(LocalObjectContainer db, int id, ISlotMap slotMap);
	}
}
#endif // !SILVERLIGHT
