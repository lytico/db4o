/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal
{
	/// <summary>TODO: Check if all time-consuming stuff is overridden!</summary>
	internal class TransactionObjectCarrier : LocalTransaction
	{
		private readonly ITransactionalIdSystem _idSystem;

		internal TransactionObjectCarrier(ObjectContainerBase container, Transaction parentTransaction
			, ITransactionalIdSystem idSystem, IReferenceSystem referenceSystem) : base(container
			, parentTransaction, idSystem, referenceSystem)
		{
			_idSystem = idSystem;
		}

		public override void Commit()
		{
		}

		// do nothing
		internal override bool SupportsVirtualFields()
		{
			return false;
		}

		public override long VersionForId(int id)
		{
			return 0;
		}

		public override Db4objects.Db4o.Internal.CommitTimestampSupport CommitTimestampSupport
			()
		{
			return null;
		}
	}
}
