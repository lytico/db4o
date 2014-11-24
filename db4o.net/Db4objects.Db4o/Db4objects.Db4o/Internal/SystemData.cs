/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class SystemData
	{
		private int _classCollectionID;

		private int _converterVersion;

		private Slot _inMemoryFreespaceSlot;

		private int _bTreeFreespaceId;

		private byte _freespaceSystem;

		private Db4oDatabase _identity;

		private int _identityId;

		private long _lastTimeStampID;

		private byte _stringEncoding;

		private int _uuidIndexId;

		private byte _idSystemType;

		private int _transactionPointer1;

		private int _transactionPointer2;

		private Slot _idSystemSlot;

		private int _idToTimestampIndexId;

		private int _timestampToIdIndexId;

		public virtual Slot IdSystemSlot()
		{
			return _idSystemSlot;
		}

		public virtual void IdSystemSlot(Slot slot)
		{
			_idSystemSlot = slot;
		}

		private ITransactionalIdSystem _freespaceIdSystem;

		public virtual void IdSystemType(byte idSystem)
		{
			_idSystemType = idSystem;
		}

		public virtual byte IdSystemType()
		{
			return _idSystemType;
		}

		public virtual int ClassCollectionID()
		{
			return _classCollectionID;
		}

		public virtual void ClassCollectionID(int id)
		{
			_classCollectionID = id;
		}

		public virtual int ConverterVersion()
		{
			return _converterVersion;
		}

		public virtual void ConverterVersion(int version)
		{
			_converterVersion = version;
		}

		public virtual int BTreeFreespaceId()
		{
			return _bTreeFreespaceId;
		}

		public virtual void BTreeFreespaceId(int id)
		{
			_bTreeFreespaceId = id;
		}

		public virtual Slot InMemoryFreespaceSlot()
		{
			return _inMemoryFreespaceSlot;
		}

		public virtual void InMemoryFreespaceSlot(Slot slot)
		{
			_inMemoryFreespaceSlot = slot;
		}

		public virtual byte FreespaceSystem()
		{
			return _freespaceSystem;
		}

		public virtual void FreespaceSystem(byte freespaceSystemtype)
		{
			_freespaceSystem = freespaceSystemtype;
		}

		public virtual Db4oDatabase Identity()
		{
			return _identity;
		}

		public virtual void Identity(Db4oDatabase identityObject)
		{
			_identity = identityObject;
		}

		public virtual long LastTimeStampID()
		{
			return _lastTimeStampID;
		}

		public virtual void LastTimeStampID(long id)
		{
			_lastTimeStampID = id;
		}

		public virtual byte StringEncoding()
		{
			return _stringEncoding;
		}

		public virtual void StringEncoding(byte encodingByte)
		{
			_stringEncoding = encodingByte;
		}

		public virtual int UuidIndexId()
		{
			return _uuidIndexId;
		}

		public virtual void UuidIndexId(int id)
		{
			_uuidIndexId = id;
		}

		public virtual void IdentityId(int id)
		{
			_identityId = id;
		}

		public virtual int IdentityId()
		{
			return _identityId;
		}

		public virtual void TransactionPointer1(int pointer)
		{
			_transactionPointer1 = pointer;
		}

		public virtual void TransactionPointer2(int pointer)
		{
			_transactionPointer2 = pointer;
		}

		public virtual int TransactionPointer1()
		{
			return _transactionPointer1;
		}

		public virtual int TransactionPointer2()
		{
			return _transactionPointer2;
		}

		public virtual void FreespaceIdSystem(ITransactionalIdSystem transactionalIdSystem
			)
		{
			_freespaceIdSystem = transactionalIdSystem;
		}

		public virtual ITransactionalIdSystem FreespaceIdSystem()
		{
			return _freespaceIdSystem;
		}

		public virtual void IdToTimestampIndexId(int idToTimestampIndexId)
		{
			_idToTimestampIndexId = idToTimestampIndexId;
		}

		public virtual int IdToTimestampIndexId()
		{
			return _idToTimestampIndexId;
		}

		public virtual void TimestampToIdIndexId(int timestampToIdIndexId)
		{
			_timestampToIdIndexId = timestampToIdIndexId;
		}

		public virtual int TimestampToIdIndexId()
		{
			return _timestampToIdIndexId;
		}
	}
}
