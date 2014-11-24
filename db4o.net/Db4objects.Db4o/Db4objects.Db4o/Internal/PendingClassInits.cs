/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	internal class PendingClassInits
	{
		private readonly Transaction _systemTransaction;

		private Collection4 _pending = new Collection4();

		private IQueue4 _members = new NonblockingQueue();

		private IQueue4 _statics = new NonblockingQueue();

		private IQueue4 _writes = new NonblockingQueue();

		private IQueue4 _inits = new NonblockingQueue();

		private bool _running = false;

		internal PendingClassInits(Transaction systemTransaction)
		{
			_systemTransaction = systemTransaction;
		}

		internal virtual void Process(ClassMetadata newClassMetadata)
		{
			if (_pending.Contains(newClassMetadata))
			{
				return;
			}
			ClassMetadata ancestor = newClassMetadata.GetAncestor();
			if (ancestor != null)
			{
				Process(ancestor);
			}
			_pending.Add(newClassMetadata);
			_members.Add(newClassMetadata);
			if (_running)
			{
				return;
			}
			_running = true;
			try
			{
				CheckInits();
				_pending = new Collection4();
			}
			finally
			{
				_running = false;
			}
		}

		private void InitializeAspects()
		{
			while (_members.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_members.Next());
				classMetadata.InitializeAspects();
				_statics.Add(classMetadata);
			}
		}

		private void CheckStatics()
		{
			InitializeAspects();
			while (_statics.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_statics.Next());
				classMetadata.StoreStaticFieldValues(_systemTransaction, true);
				_writes.Add(classMetadata);
				InitializeAspects();
			}
		}

		private void CheckWrites()
		{
			CheckStatics();
			while (_writes.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_writes.Next());
				classMetadata.SetStateDirty();
				classMetadata.Write(_systemTransaction);
				_inits.Add(classMetadata);
				CheckStatics();
			}
		}

		private void CheckInits()
		{
			CheckWrites();
			while (_inits.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_inits.Next());
				classMetadata.InitConfigOnUp(_systemTransaction);
				CheckWrites();
			}
		}
	}
}
