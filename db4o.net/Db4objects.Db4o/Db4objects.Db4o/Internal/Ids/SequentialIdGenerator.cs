/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class SequentialIdGenerator
	{
		private readonly int _minValidId;

		private readonly int _maxValidId;

		private int _idGenerator;

		private bool _overflow;

		private int _lastIdGenerator;

		private readonly IFunction4 _findFreeId;

		public SequentialIdGenerator(IFunction4 findFreeId, int initialValue, int minValidId
			, int maxValidId)
		{
			_findFreeId = findFreeId;
			_minValidId = minValidId;
			_maxValidId = maxValidId;
			InitializeGenerator(initialValue);
		}

		public SequentialIdGenerator(IFunction4 findFreeId, int minValidId, int maxValidId
			) : this(findFreeId, minValidId - 1, minValidId, maxValidId)
		{
		}

		public virtual void Read(ByteArrayBuffer buffer)
		{
			InitializeGenerator(buffer.ReadInt());
		}

		private void InitializeGenerator(int val)
		{
			if (val < 0)
			{
				_overflow = true;
				_idGenerator = -val;
			}
			else
			{
				_idGenerator = val;
			}
			_lastIdGenerator = _idGenerator;
		}

		public virtual void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(PersistentGeneratorValue());
		}

		public virtual int PersistentGeneratorValue()
		{
			return _overflow ? -_idGenerator : _idGenerator;
		}

		public virtual int NewId()
		{
			AdjustIdGenerator(_idGenerator);
			if (!_overflow)
			{
				return _idGenerator;
			}
			int id = (((int)_findFreeId.Apply(_idGenerator)));
			if (id > 0)
			{
				AdjustIdGenerator(id - 1);
				return id;
			}
			id = (((int)_findFreeId.Apply(_minValidId)));
			if (id > 0)
			{
				AdjustIdGenerator(id - 1);
				return id;
			}
			throw new Db4oFatalException("Out of IDs");
		}

		private void AdjustIdGenerator(int id)
		{
			if (id == _maxValidId)
			{
				_idGenerator = _minValidId;
				_overflow = true;
				return;
			}
			_idGenerator = id + 1;
		}

		public virtual int MarshalledLength()
		{
			return Const4.IntLength;
		}

		public virtual bool IsDirty()
		{
			return _idGenerator != _lastIdGenerator;
		}

		public virtual void SetClean()
		{
			_lastIdGenerator = _idGenerator;
		}
	}
}
