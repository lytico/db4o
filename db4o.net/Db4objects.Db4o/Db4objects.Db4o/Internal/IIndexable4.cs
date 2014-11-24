/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public interface IIndexable4 : IComparable4, ILinkLengthAware
	{
		object ReadIndexEntry(IContext context, ByteArrayBuffer reader);

		void WriteIndexEntry(IContext context, ByteArrayBuffer writer, object obj);

		void DefragIndexEntry(DefragmentContextImpl context);
	}
}
