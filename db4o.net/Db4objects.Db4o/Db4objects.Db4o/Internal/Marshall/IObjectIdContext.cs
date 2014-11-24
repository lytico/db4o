/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public interface IObjectIdContext : IHandlerVersionContext, IInternalReadContext
	{
		int ObjectId();
	}
}
