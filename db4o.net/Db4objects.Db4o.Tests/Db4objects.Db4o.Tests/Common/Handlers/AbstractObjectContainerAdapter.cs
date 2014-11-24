/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public abstract class AbstractObjectContainerAdapter : IObjectContainerAdapter
	{
		protected IExtObjectContainer db;

		public virtual IObjectContainerAdapter ForContainer(IExtObjectContainer db)
		{
			this.db = db;
			return this;
		}

		public virtual void Commit()
		{
			db.Commit();
		}

		public virtual void Delete(object obj)
		{
			db.Delete(obj);
		}

		public virtual IQuery Query()
		{
			return db.Query();
		}

		public AbstractObjectContainerAdapter() : base()
		{
		}

		public virtual object ObjectContainer()
		{
			return db;
		}

		public abstract void Store(object arg1);

		public abstract void Store(object arg1, int arg2);
	}
}
