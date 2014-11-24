/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public abstract class AbstractReadContext : AbstractBufferContext, IInternalReadContext
	{
		protected IActivationDepth _activationDepth = UnknownActivationDepth.Instance;

		private bool _lastReferenceReadWasReallyNull = false;

		protected AbstractReadContext(Transaction transaction, IReadBuffer buffer) : base
			(transaction, buffer)
		{
		}

		protected AbstractReadContext(Transaction transaction) : this(transaction, null)
		{
		}

		public object Read(ITypeHandler4 handlerType)
		{
			return ReadObject(handlerType);
		}

		public object ReadObject(ITypeHandler4 handlerType)
		{
			if (null == handlerType)
			{
				throw new ArgumentNullException();
			}
			ITypeHandler4 handler = HandlerRegistry.CorrectHandlerVersion(this, handlerType);
			return SlotFormat().DoWithSlotIndirection(this, handler, new _IClosure4_38(this, 
				handler));
		}

		private sealed class _IClosure4_38 : IClosure4
		{
			public _IClosure4_38(AbstractReadContext _enclosing, ITypeHandler4 handler)
			{
				this._enclosing = _enclosing;
				this.handler = handler;
			}

			public object Run()
			{
				return this._enclosing.ReadAtCurrentSeekPosition(handler);
			}

			private readonly AbstractReadContext _enclosing;

			private readonly ITypeHandler4 handler;
		}

		public virtual object ReadAtCurrentSeekPosition(ITypeHandler4 handler)
		{
			if (Handlers4.UseDedicatedSlot(this, handler))
			{
				return ReadObject();
			}
			return Handlers4.ReadValueType(this, handler);
		}

		public object ReadObject()
		{
			int objectId = ReadInt();
			if (objectId == 0)
			{
				_lastReferenceReadWasReallyNull = true;
				return null;
			}
			_lastReferenceReadWasReallyNull = false;
			if (objectId == Const4.InvalidObjectId)
			{
				return null;
			}
			ClassMetadata classMetadata = ClassMetadataForObjectId(objectId);
			if (null == classMetadata)
			{
				// TODO: throw here
				return null;
			}
			IActivationDepth depth = ActivationDepth().Descend(classMetadata);
			if (PeekPersisted())
			{
				return Container().PeekPersisted(Transaction(), objectId, depth, false);
			}
			object obj = Container().GetByID2(Transaction(), objectId);
			if (null == obj)
			{
				return null;
			}
			// this is OK for boxed value types. They will not be added
			// to the list, since they will not be found in the ID tree.
			Container().StillToActivate(Container().ActivationContextFor(Transaction(), obj, 
				depth));
			return obj;
		}

		private ClassMetadata ClassMetadataForObjectId(int objectId)
		{
			// TODO: This method is *very* costly as is, since it reads
			//       the whole slot once and doesn't reuse it. Optimize.
			HardObjectReference hardRef = Container().GetHardObjectReferenceById(Transaction(
				), objectId);
			if (null == hardRef || hardRef._reference == null)
			{
				// com.db4o.db4ounit.common.querying.CascadeDeleteDeleted
				return null;
			}
			return hardRef._reference.ClassMetadata();
		}

		protected virtual bool PeekPersisted()
		{
			return false;
		}

		public virtual IActivationDepth ActivationDepth()
		{
			return _activationDepth;
		}

		public virtual void ActivationDepth(IActivationDepth depth)
		{
			_activationDepth = depth;
		}

		public virtual IReadWriteBuffer ReadIndirectedBuffer()
		{
			int address = ReadInt();
			int length = ReadInt();
			if (address == 0)
			{
				return null;
			}
			return Container().DecryptedBufferByAddress(address, length);
		}

		public virtual bool LastReferenceReadWasReallyNull()
		{
			return _lastReferenceReadWasReallyNull;
		}

		public virtual void NotifyNullReferenceSkipped()
		{
			_lastReferenceReadWasReallyNull = true;
		}
	}
}
