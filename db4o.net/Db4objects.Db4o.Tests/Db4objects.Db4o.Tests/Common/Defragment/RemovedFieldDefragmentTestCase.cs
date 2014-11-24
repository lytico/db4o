/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class RemovedFieldDefragmentTestCase : AbstractDb4oTestCase
	{
		public class Before
		{
			public int _id;

			public Before(int id)
			{
				_id = id;
			}
		}

		public class After
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new RemovedFieldDefragmentTestCase.Before(42));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRetrieval()
		{
			Fixture().ResetConfig();
			IConfiguration config = Fixture().Config();
			IReflector reflector = new ExcludingReflector(new Type[] { typeof(RemovedFieldDefragmentTestCase.Before
				) });
			config.ReflectWith(reflector);
			TypeAlias alias = new TypeAlias(typeof(RemovedFieldDefragmentTestCase.Before), typeof(
				RemovedFieldDefragmentTestCase.After));
			config.AddAlias(alias);
			Defragment();
			RemovedFieldDefragmentTestCase.After after = ((RemovedFieldDefragmentTestCase.After
				)RetrieveOnlyInstance(typeof(RemovedFieldDefragmentTestCase.After)));
			config = Fixture().Config();
			config.ReflectWith(new ExcludingReflector(new Type[] {  }));
			config.RemoveAlias(alias);
			Reopen();
			RemovedFieldDefragmentTestCase.Before before = ((RemovedFieldDefragmentTestCase.Before
				)RetrieveOnlyInstance(typeof(RemovedFieldDefragmentTestCase.Before)));
			Assert.AreEqual(42, before._id);
		}
	}
}
