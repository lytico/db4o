/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public interface IReferenceSystemFactory
	{
		IReferenceSystem NewReferenceSystem(IInternalObjectContainer container);
	}
}
