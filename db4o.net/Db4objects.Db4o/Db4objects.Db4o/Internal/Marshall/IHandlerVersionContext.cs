/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public interface IHandlerVersionContext : IContext
	{
		int HandlerVersion();

		Db4objects.Db4o.Internal.Marshall.SlotFormat SlotFormat();
	}
}
