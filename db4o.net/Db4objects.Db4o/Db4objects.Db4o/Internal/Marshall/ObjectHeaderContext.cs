/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ObjectHeaderContext : AbstractReadContext, IMarshallingInfo, IHandlerVersionContext
	{
		protected ObjectHeader _objectHeader;

		private int _declaredAspectCount;

		public ObjectHeaderContext(Transaction transaction, IReadBuffer buffer, ObjectHeader
			 objectHeader) : base(transaction, buffer)
		{
			_objectHeader = objectHeader;
		}

		public ObjectHeaderAttributes HeaderAttributes()
		{
			return _objectHeader._headerAttributes;
		}

		public bool IsNull(int fieldIndex)
		{
			return HeaderAttributes().IsNull(fieldIndex);
		}

		public override int HandlerVersion()
		{
			return _objectHeader.HandlerVersion();
		}

		public virtual void BeginSlot()
		{
		}

		// do nothing
		public virtual ContextState SaveState()
		{
			return new ContextState(Offset());
		}

		public virtual void RestoreState(ContextState state)
		{
			Seek(state._offset);
		}

		public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _objectHeader.ClassMetadata();
		}

		public virtual int DeclaredAspectCount()
		{
			return _declaredAspectCount;
		}

		public virtual void DeclaredAspectCount(int count)
		{
			_declaredAspectCount = count;
		}
	}
}
