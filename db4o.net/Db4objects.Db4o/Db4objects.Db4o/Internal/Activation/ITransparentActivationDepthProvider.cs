/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <exclude></exclude>
	public interface ITransparentActivationDepthProvider : IActivationDepthProvider
	{
		void EnableTransparentPersistenceSupportFor(IInternalObjectContainer container, IRollbackStrategy
			 withRollbackStrategy);

		void AddModified(object @object, Transaction inTransaction);

		void RemoveModified(object @object, Transaction inTransaction);
	}
}
