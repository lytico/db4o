/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <summary>marker interface for special db4o datatypes</summary>
	/// <exclude></exclude>
	public interface IDb4oTypeImpl : ITransactionAware
	{
		object CreateDefault(Transaction trans);

		bool HasClassIndex();

		void SetObjectReference(ObjectReference @ref);
	}
}
