/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Marshall
{
	/// <summary>
	/// common functionality for
	/// <see cref="IReadContext">IReadContext</see>
	/// and
	/// <see cref="IWriteContext">IWriteContext</see>
	/// and
	/// <see cref="Db4objects.Db4o.Internal.Delete.IDeleteContext">Db4objects.Db4o.Internal.Delete.IDeleteContext
	/// 	</see>
	/// 
	/// </summary>
	public interface IContext
	{
		IObjectContainer ObjectContainer();

		Db4objects.Db4o.Internal.Transaction Transaction();
	}
}
