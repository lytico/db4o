/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeConfiguration
	{
		public static readonly Db4objects.Db4o.Internal.Btree.BTreeConfiguration Default = 
			new Db4objects.Db4o.Internal.Btree.BTreeConfiguration(null, 20, true);

		public readonly ITransactionalIdSystem _idSystem;

		public readonly SlotChangeFactory _slotChangeFactory;

		public readonly bool _canEnlistWithTransaction;

		public readonly int _cacheSize;

		public BTreeConfiguration(ITransactionalIdSystem idSystem, SlotChangeFactory slotChangeFactory
			, int cacheSize, bool canEnlistWithTransaction)
		{
			_idSystem = idSystem;
			_slotChangeFactory = slotChangeFactory;
			_canEnlistWithTransaction = canEnlistWithTransaction;
			_cacheSize = cacheSize;
		}

		public BTreeConfiguration(ITransactionalIdSystem idSystem, int cacheSize, bool canEnlistWithTransaction
			) : this(idSystem, SlotChangeFactory.SystemObjects, cacheSize, canEnlistWithTransaction
			)
		{
		}
	}
}
