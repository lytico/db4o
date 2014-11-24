/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public interface IObjectContainerAdapter
	{
		void Store(object obj);

		void Store(object obj, int depth);

		void Commit();

		void Delete(object obj);

		IQuery Query();

		IObjectContainerAdapter ForContainer(IExtObjectContainer db);

		object ObjectContainer();
	}
}
