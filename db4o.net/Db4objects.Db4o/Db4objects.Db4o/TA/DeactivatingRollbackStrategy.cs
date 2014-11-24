/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.TA
{
	/// <summary>RollbackStrategy to deactivate all activated objects on rollback.</summary>
	/// <remarks>RollbackStrategy to deactivate all activated objects on rollback.</remarks>
	/// <seealso cref="TransparentPersistenceSupport">TransparentPersistenceSupport</seealso>
	public class DeactivatingRollbackStrategy : IRollbackStrategy
	{
		/// <summary>deactivates each object.</summary>
		/// <remarks>deactivates each object.</remarks>
		public virtual void Rollback(IObjectContainer container, object obj)
		{
			container.Ext().Deactivate(obj);
		}
	}
}
