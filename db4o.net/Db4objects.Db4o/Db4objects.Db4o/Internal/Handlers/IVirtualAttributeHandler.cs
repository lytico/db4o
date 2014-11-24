/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public interface IVirtualAttributeHandler
	{
		void ReadVirtualAttributes(ObjectReferenceContext context);
	}
}
