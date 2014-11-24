/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class FileHeaderVariablePart
	{
		protected readonly LocalObjectContainer _container;

		public abstract IRunnable Commit(bool shuttingDown);

		public abstract void Read(int variablePartAddress, int variablePartLength);

		protected FileHeaderVariablePart(LocalObjectContainer container)
		{
			_container = container;
		}

		public byte GetIdentifier()
		{
			return Const4.Header;
		}

		protected Db4objects.Db4o.Internal.SystemData SystemData()
		{
			return _container.SystemData();
		}

		protected Slot AllocateSlot(int length)
		{
			Slot reusedSlot = _container.FreespaceManager().AllocateSafeSlot(length);
			if (reusedSlot != null)
			{
				return reusedSlot;
			}
			return _container.AppendBytes(length);
		}

		public virtual void ReadIdentity(LocalTransaction trans)
		{
			LocalObjectContainer file = trans.LocalContainer();
			Db4oDatabase identity = Debug4.staticIdentity ? Db4oDatabase.StaticIdentity : (Db4oDatabase
				)file.GetByID(trans, SystemData().IdentityId());
			if (null != identity)
			{
				file.Activate(trans, identity, new FixedActivationDepth(2));
				SystemData().Identity(identity);
			}
		}

		// TODO: What now?
		// Apparently we get this state after defragment
		// and defragment then sets the identity.
		// If we blindly generate a new identity here,
		// ObjectUpdateFileSizeTestCase reports trouble.
		public abstract int MarshalledLength();
	}
}
