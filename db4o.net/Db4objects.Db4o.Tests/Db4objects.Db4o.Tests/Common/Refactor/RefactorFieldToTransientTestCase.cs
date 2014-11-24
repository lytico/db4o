/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class RefactorFieldToTransientTestCase : AbstractDb4oTestCase
	{
		public class Before
		{
			public int _id;

			public Before(int id)
			{
				// COR-1721
				_id = id;
			}
		}

		public class After
		{
			[System.NonSerialized]
			public int _id;

			public After(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new RefactorFieldToTransientTestCase.Before(42));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRetrieval()
		{
			Fixture().ResetConfig();
			IConfiguration config = Fixture().Config();
			IReflector reflector = new ExcludingReflector(new Type[] { typeof(RefactorFieldToTransientTestCase.Before
				) });
			config.ReflectWith(reflector);
			TypeAlias alias = new TypeAlias(typeof(RefactorFieldToTransientTestCase.Before), 
				typeof(RefactorFieldToTransientTestCase.After));
			config.AddAlias(alias);
			Reopen();
			RefactorFieldToTransientTestCase.After after = ((RefactorFieldToTransientTestCase.After
				)RetrieveOnlyInstance(typeof(RefactorFieldToTransientTestCase.After)));
			Assert.AreEqual(0, after._id);
			config = Fixture().Config();
			config.ReflectWith(new ExcludingReflector(new Type[] {  }));
			config.RemoveAlias(alias);
			Reopen();
			RefactorFieldToTransientTestCase.Before before = ((RefactorFieldToTransientTestCase.Before
				)RetrieveOnlyInstance(typeof(RefactorFieldToTransientTestCase.Before)));
			Assert.AreEqual(42, before._id);
		}
	}
}
