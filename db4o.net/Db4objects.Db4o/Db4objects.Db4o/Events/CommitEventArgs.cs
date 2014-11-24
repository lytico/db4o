/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Events
{
	/// <summary>Arguments for commit time related events.</summary>
	/// <remarks>Arguments for commit time related events.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public class CommitEventArgs : TransactionalEventArgs
	{
		private readonly CallbackObjectInfoCollections _collections;

		private readonly bool _isOwnCommit;

		public CommitEventArgs(Transaction transaction, CallbackObjectInfoCollections collections
			, bool isOwnCommit) : base(transaction)
		{
			_collections = collections;
			_isOwnCommit = isOwnCommit;
		}

		/// <summary>Returns a iteration</summary>
		public virtual IObjectInfoCollection Added
		{
			get
			{
				return _collections.added;
			}
		}

		public virtual IObjectInfoCollection Deleted
		{
			get
			{
				return _collections.deleted;
			}
		}

		public virtual IObjectInfoCollection Updated
		{
			get
			{
				return _collections.updated;
			}
		}

		public virtual bool IsOwnCommit()
		{
			return _isOwnCommit;
		}
	}
}
