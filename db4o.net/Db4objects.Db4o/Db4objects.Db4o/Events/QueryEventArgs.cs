/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Events
{
	/// <summary>
	/// Arguments for
	/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
	/// related events.
	/// </summary>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public class QueryEventArgs : TransactionalEventArgs
	{
		private IQuery _query;

		/// <summary>
		/// Creates a new instance for the specified
		/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
		/// instance.
		/// </summary>
		public QueryEventArgs(Transaction transaction, IQuery q) : base(transaction)
		{
			_query = q;
		}

		/// <summary>
		/// The
		/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
		/// which triggered the event.
		/// </summary>
		public virtual IQuery Query
		{
			get
			{
				return _query;
			}
		}
	}
}
