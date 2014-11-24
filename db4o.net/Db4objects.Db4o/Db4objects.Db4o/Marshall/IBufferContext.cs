/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Marshall
{
	/// <exclude></exclude>
	public interface IBufferContext : IReadBuffer, IContext
	{
		IReadBuffer Buffer();
	}
}
