/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class MultidimensionalArrayHandler0 : MultidimensionalArrayHandler3
	{
		protected override ArrayVersionHelper CreateVersionHelper()
		{
			return new ArrayVersionHelper0();
		}

		public override object Read(IReadContext readContext)
		{
			IInternalReadContext context = (IInternalReadContext)readContext;
			ByteArrayBuffer buffer = (ByteArrayBuffer)context.ReadIndirectedBuffer();
			if (buffer == null)
			{
				return null;
			}
			// With the following line we ask the context to work with 
			// a different buffer. Should this logic ever be needed by
			// a user handler, it should be implemented by using a Queue
			// in the UnmarshallingContext.
			// The buffer has to be set back from the outside!  See below
			IReadBuffer contextBuffer = context.Buffer(buffer);
			object array = base.Read(context);
			// The context buffer has to be set back.
			context.Buffer(contextBuffer);
			return array;
		}

		public override void Defragment(IDefragmentContext context)
		{
			ArrayHandler0.Defragment(context, this);
		}
	}
}
