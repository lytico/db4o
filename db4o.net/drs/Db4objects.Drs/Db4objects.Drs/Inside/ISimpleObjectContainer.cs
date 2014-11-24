/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;

namespace Db4objects.Drs.Inside
{
	public interface ISimpleObjectContainer
	{
		void Activate(object @object);

		void Commit();

		void Delete(object obj);

		void DeleteAllInstances(Type clazz);

		IObjectSet GetStoredObjects(Type type);

		/// <summary>Will cascade to save the whole graph of objects</summary>
		void StoreNew(object o);

		/// <summary>Updating won't cascade.</summary>
		/// <remarks>Updating won't cascade.</remarks>
		void Update(object o);
	}
}
