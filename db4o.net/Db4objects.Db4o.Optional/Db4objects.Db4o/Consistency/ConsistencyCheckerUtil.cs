/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Sharpen.Util;

namespace Db4objects.Db4o.Consistency
{
	/// <exclude></exclude>
	public sealed class ConsistencyCheckerUtil
	{
		public static IDictionary TypesFor(LocalObjectContainer db, Sharpen.Util.ISet ids
			)
		{
			IDictionary id2clazzes = new Hashtable();
			ClassMetadataIterator iter = db.ClassCollection().Iterator();
			while (iter.MoveNext())
			{
				for (IEnumerator idIter = ids.GetEnumerator(); idIter.MoveNext(); )
				{
					int id = ((int)idIter.Current);
					ClassMetadata clazz = iter.CurrentClass();
					BTree btree = BTreeClassIndexStrategy.Btree(clazz);
					if (btree.Search(db.SystemTransaction(), id) != null)
					{
						Sharpen.Util.ISet clazzes = ((Sharpen.Util.ISet)id2clazzes[id]);
						if (clazzes == null)
						{
							clazzes = new HashSet();
							id2clazzes[id] = clazzes;
						}
						clazzes.Add(clazz);
					}
				}
			}
			IDictionary id2clazz = new Hashtable();
			for (IEnumerator idIter = id2clazzes.Keys.GetEnumerator(); idIter.MoveNext(); )
			{
				int id = ((int)idIter.Current);
				Sharpen.Util.ISet clazzes = ((Sharpen.Util.ISet)id2clazzes[id]);
				ClassMetadata mostSpecific = null;
				for (IEnumerator curClazzIter = clazzes.GetEnumerator(); curClazzIter.MoveNext(); )
				{
					ClassMetadata curClazz = ((ClassMetadata)curClazzIter.Current);
					for (IEnumerator cmpClazzIter = clazzes.GetEnumerator(); cmpClazzIter.MoveNext(); )
					{
						ClassMetadata cmpClazz = ((ClassMetadata)cmpClazzIter.Current);
						if (curClazz.Equals(cmpClazz._ancestor))
						{
							goto OUTER_continue;
						}
					}
					mostSpecific = curClazz;
					break;
OUTER_continue: ;
				}
OUTER_break: ;
				id2clazz[id] = mostSpecific;
			}
			return id2clazz;
		}

		private ConsistencyCheckerUtil()
		{
		}
	}
}
