/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Constraints;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class UniqueConstraintOnServerTestCase : Db4oClientServerTestCase, ICustomClientServerConfiguration
	{
		public static void Main(string[] args)
		{
			new UniqueConstraintOnServerTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UniqueConstraintOnServerTestCase.UniqueId)).ObjectField
				("id").Indexed(true);
			config.Add(new UniqueFieldValueConstraint(typeof(UniqueConstraintOnServerTestCase.UniqueId
				), "id"));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
		}

		// do nothing
		public virtual void TestWorksForUniqueItems()
		{
			Store(new UniqueConstraintOnServerTestCase.UniqueId(1));
			Store(new UniqueConstraintOnServerTestCase.UniqueId(2));
			Store(new UniqueConstraintOnServerTestCase.UniqueId(3));
			Commit();
		}

		public virtual void TestNotUniqueItems()
		{
			Store(new UniqueConstraintOnServerTestCase.UniqueId(1));
			Store(new UniqueConstraintOnServerTestCase.UniqueId(1));
			bool exceptionWasThrown = false;
			try
			{
				Commit();
			}
			catch (UniqueFieldValueConstraintViolationException)
			{
				exceptionWasThrown = true;
			}
			Assert.IsTrue(exceptionWasThrown);
			Db().Rollback();
		}

		public class UniqueId
		{
			public int id;

			public UniqueId(int id)
			{
				this.id = id;
			}
		}
	}
}
#endif // !SILVERLIGHT
