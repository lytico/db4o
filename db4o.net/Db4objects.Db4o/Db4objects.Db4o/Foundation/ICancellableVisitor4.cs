/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	public interface ICancellableVisitor4
	{
		/// <returns>true to continue traversal, false otherwise</returns>
		bool Visit(object obj);
	}
}
