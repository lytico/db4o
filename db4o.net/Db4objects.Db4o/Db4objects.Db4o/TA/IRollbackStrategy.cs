/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.TA
{
	/// <summary>Interface defining rollback behavior when Transparent Persistence mode is on.
	/// 	</summary>
	/// <remarks>Interface defining rollback behavior when Transparent Persistence mode is on.
	/// 	</remarks>
	/// <seealso cref="TransparentPersistenceSupport">TransparentPersistenceSupport</seealso>
	public interface IRollbackStrategy
	{
		/// <summary>Method to be called per TP-enabled object when the transaction is rolled back.
		/// 	</summary>
		/// <remarks>Method to be called per TP-enabled object when the transaction is rolled back.
		/// 	</remarks>
		/// <param name="container">current ObjectContainer</param>
		/// <param name="obj">TP-enabled object</param>
		void Rollback(IObjectContainer container, object obj);
	}
}
