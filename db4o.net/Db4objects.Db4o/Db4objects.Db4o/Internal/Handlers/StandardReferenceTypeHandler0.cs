/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class StandardReferenceTypeHandler0 : StandardReferenceTypeHandler
	{
		protected override IMarshallingInfo EnsureFieldList(IMarshallingInfo context)
		{
			return new _IMarshallingInfo_16(context);
		}

		private sealed class _IMarshallingInfo_16 : IMarshallingInfo
		{
			public _IMarshallingInfo_16(IMarshallingInfo context)
			{
				this.context = context;
			}

			public void DeclaredAspectCount(int count)
			{
				context.DeclaredAspectCount(count);
			}

			public int DeclaredAspectCount()
			{
				return context.DeclaredAspectCount();
			}

			public bool IsNull(int fieldIndex)
			{
				return false;
			}

			public ClassMetadata ClassMetadata()
			{
				return context.ClassMetadata();
			}

			public IReadBuffer Buffer()
			{
				return context.Buffer();
			}

			public void BeginSlot()
			{
				context.BeginSlot();
			}

			private readonly IMarshallingInfo context;
		}
	}
}
