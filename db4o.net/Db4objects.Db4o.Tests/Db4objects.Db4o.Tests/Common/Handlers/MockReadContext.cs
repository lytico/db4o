/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class MockReadContext : MockMarshallingContext, IReadContext
	{
		public MockReadContext(IObjectContainer objectContainer) : base(objectContainer)
		{
		}

		public MockReadContext(MockWriteContext writeContext) : this(writeContext.ObjectContainer
			())
		{
			writeContext._header.CopyTo(_header, 0, 0, writeContext._header.Length());
			writeContext._payLoad.CopyTo(_payLoad, 0, 0, writeContext._payLoad.Length());
		}

		public virtual object ReadObject(ITypeHandler4 handler)
		{
			return Handlers4.ReadValueType(this, handler);
		}

		public virtual BitMap4 ReadBitMap(int bitCount)
		{
			BitMap4 map = new BitMap4(_current._buffer, _current._offset, bitCount);
			_current.Seek(_current.Offset() + map.MarshalledLength());
			return map;
		}
	}
}
