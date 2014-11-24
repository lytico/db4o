/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Collections;
using System.Collections.Generic;

namespace Db4objects.Db4o.Linq
{
	/// <summary>
	/// IDb4oLinqQuery is the query type of Linq to db4o. Standard query operators
	/// are defined in <see cref="Db4objects.Db4o.Linq.Db4oLinqQueryExtensions">Db4oLinqQueryExtensions</see>.
	/// </summary>
	/// <typeparam name="T">The type of the objects that are queried from the database.</typeparam>
	public interface IDb4oLinqQuery<T> : IDb4oLinqQuery, IEnumerable<T>
	{
	}

	public interface IDb4oLinqQuery : IEnumerable
	{
	}
}
