/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Events
{
	/// <summary>Arguments for object related events.</summary>
	/// <remarks>Arguments for object related events.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public abstract class ObjectEventArgs : TransactionalEventArgs
	{
		/// <summary>Creates a new instance for the specified object.</summary>
		/// <remarks>Creates a new instance for the specified object.</remarks>
		protected ObjectEventArgs(Transaction transaction) : base(transaction)
		{
		}

		/// <summary>The object that triggered this event.</summary>
		/// <remarks>The object that triggered this event.</remarks>
		public abstract object Object
		{
			get;
		}
	}
}
