/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public abstract class MockMarshallingContext
	{
		private readonly IObjectContainer _objectContainer;

		internal readonly ByteArrayBuffer _header;

		internal readonly ByteArrayBuffer _payLoad;

		protected ByteArrayBuffer _current;

		public MockMarshallingContext(IObjectContainer objectContainer)
		{
			_objectContainer = objectContainer;
			_header = new ByteArrayBuffer(1000);
			_payLoad = new ByteArrayBuffer(1000);
			_current = _header;
		}

		public virtual IWriteBuffer NewBuffer(int length)
		{
			return new ByteArrayBuffer(length);
		}

		public virtual IObjectContainer ObjectContainer()
		{
			return _objectContainer;
		}

		public virtual void UseVariableLength()
		{
			_current = _payLoad;
		}

		public virtual byte ReadByte()
		{
			return _current.ReadByte();
		}

		public virtual void ReadBytes(byte[] bytes)
		{
			_current.ReadBytes(bytes);
		}

		public virtual int ReadInt()
		{
			return _current.ReadInt();
		}

		public virtual long ReadLong()
		{
			return _current.ReadLong();
		}

		public virtual void WriteByte(byte b)
		{
			_current.WriteByte(b);
		}

		public virtual void WriteInt(int i)
		{
			_current.WriteInt(i);
		}

		public virtual void WriteLong(long l)
		{
			_current.WriteLong(l);
		}

		public virtual void WriteBytes(byte[] bytes)
		{
			_current.WriteBytes(bytes);
		}

		public virtual object ReadObject()
		{
			int id = ReadInt();
			object obj = Container().GetByID(Transaction(), id);
			ObjectContainer().Activate(obj, int.MaxValue);
			return obj;
		}

		public virtual void WriteObject(object obj)
		{
			int id = Container().StoreInternal(Transaction(), obj, false);
			WriteInt(id);
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return Container().Transaction;
		}

		protected virtual ObjectContainerBase Container()
		{
			return ((IInternalObjectContainer)_objectContainer).Container;
		}

		public virtual int Offset()
		{
			return _current.Offset();
		}

		public virtual void Seek(int offset)
		{
			_current.Seek(offset);
		}

		public virtual void SeekCurrentInt()
		{
			Seek(ReadInt());
		}
	}
}
