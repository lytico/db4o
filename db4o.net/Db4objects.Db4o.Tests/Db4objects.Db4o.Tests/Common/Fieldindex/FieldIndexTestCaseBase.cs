/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public abstract class FieldIndexTestCaseBase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public FieldIndexTestCaseBase() : base()
		{
		}

		protected override void Configure(IConfiguration config)
		{
			IndexField(config, typeof(FieldIndexItem), "foo");
		}

		protected abstract override void Store();

		protected virtual void StoreItems(int[] foos)
		{
			for (int i = 0; i < foos.Length; i++)
			{
				Store(new FieldIndexItem(foos[i]));
			}
		}

		protected virtual IQuery CreateQuery(int id)
		{
			IQuery q = CreateItemQuery();
			q.Descend("foo").Constrain(id);
			return q;
		}

		protected virtual IQuery CreateItemQuery()
		{
			return CreateQuery(typeof(FieldIndexItem));
		}

		protected virtual IQuery CreateQuery(Type clazz)
		{
			return CreateQuery(Trans(), clazz);
		}

		protected virtual IQuery CreateQuery(Transaction trans, Type clazz)
		{
			IQuery q = CreateQuery(trans);
			q.Constrain(clazz);
			return q;
		}

		protected virtual IQuery CreateItemQuery(Transaction trans)
		{
			IQuery q = CreateQuery(trans);
			q.Constrain(typeof(FieldIndexItem));
			return q;
		}

		private IQuery CreateQuery(Transaction trans)
		{
			return Container().Query(trans);
		}
	}
}
