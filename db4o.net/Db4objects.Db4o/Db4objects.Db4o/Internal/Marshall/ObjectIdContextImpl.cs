/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ObjectIdContextImpl : ObjectHeaderContext, IObjectIdContext
	{
		private readonly int _id;

		public ObjectIdContextImpl(Transaction transaction, IReadBuffer buffer, ObjectHeader
			 objectHeader, int id) : base(transaction, buffer, objectHeader)
		{
			_id = id;
		}

		public virtual int ObjectId()
		{
			return _id;
		}
	}
}
