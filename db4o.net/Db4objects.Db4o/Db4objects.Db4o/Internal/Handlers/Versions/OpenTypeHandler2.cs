/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Versions;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers.Versions
{
	/// <exclude></exclude>
	public class OpenTypeHandler2 : OpenTypeHandler7
	{
		public OpenTypeHandler2(ObjectContainerBase container) : base(container)
		{
		}

		protected override void SeekSecondaryOffset(IReadBuffer buffer, ITypeHandler4 typeHandler
			)
		{
			if (Handlers4.HandlesPrimitiveArray(typeHandler))
			{
				buffer.Seek(buffer.ReadInt());
			}
		}
	}
}
