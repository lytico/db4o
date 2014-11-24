/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class FreespaceCommitter
	{
		public static readonly Db4objects.Db4o.Internal.Ids.FreespaceCommitter DoNothing = 
			new FreespaceCommitter.NullFreespaceCommitter();

		private readonly IList _freeToUserFreespaceSystem = new ArrayList();

		private readonly IList _freeToSystemFreespaceSystem = new ArrayList();

		private readonly IFreespaceManager _freespaceManager;

		private ITransactionalIdSystem _transactionalIdSystem;

		public FreespaceCommitter(IFreespaceManager freespaceManager)
		{
			_freespaceManager = freespaceManager == null ? NullFreespaceManager.Instance : freespaceManager;
		}

		public virtual void Commit()
		{
			Apply(_freeToUserFreespaceSystem);
			_freespaceManager.BeginCommit();
			_freespaceManager.Commit();
			_transactionalIdSystem.AccumulateFreeSlots(this, true);
			Apply(_freeToSystemFreespaceSystem);
			_freespaceManager.EndCommit();
		}

		private void Apply(IList toFree)
		{
			for (IEnumerator slotIter = toFree.GetEnumerator(); slotIter.MoveNext(); )
			{
				Slot slot = ((Slot)slotIter.Current);
				_freespaceManager.Free(slot);
			}
			toFree.Clear();
		}

		public virtual void TransactionalIdSystem(ITransactionalIdSystem transactionalIdSystem
			)
		{
			_transactionalIdSystem = transactionalIdSystem;
		}

		private class NullFreespaceCommitter : FreespaceCommitter
		{
			public NullFreespaceCommitter() : base(NullFreespaceManager.Instance)
			{
			}

			public override void Commit()
			{
			}
			// do nothing
		}

		public virtual void DelayedFree(Slot slot, bool freeToSystemFreeSpaceSystem)
		{
			if (freeToSystemFreeSpaceSystem)
			{
				_freeToSystemFreespaceSystem.Add(slot);
			}
			else
			{
				_freeToUserFreespaceSystem.Add(slot);
			}
		}
	}
}
