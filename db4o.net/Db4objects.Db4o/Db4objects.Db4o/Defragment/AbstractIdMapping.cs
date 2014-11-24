/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Base class for defragment ID mappings.</summary>
	/// <remarks>Base class for defragment ID mappings.</remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public abstract class AbstractIdMapping : IIdMapping
	{
		private Hashtable4 _classIDs = new Hashtable4();

		public void MapId(int origID, int mappedID, bool isClassID)
		{
			if (isClassID)
			{
				MapClassIDs(origID, mappedID);
				return;
			}
			MapNonClassIDs(origID, mappedID);
		}

		protected virtual int MappedClassID(int origID)
		{
			object obj = _classIDs.Get(origID);
			if (obj == null)
			{
				return 0;
			}
			return ((int)obj);
		}

		private void MapClassIDs(int oldID, int newID)
		{
			_classIDs.Put(oldID, newID);
		}

		protected abstract void MapNonClassIDs(int origID, int mappedID);

		public abstract int AddressForId(int arg1);

		public abstract void Close();

		public abstract void Commit();

		public abstract void MapId(int arg1, Slot arg2);

		public abstract int MappedId(int arg1);

		public abstract void Open();

		public abstract IVisitable SlotChanges();
	}
}
