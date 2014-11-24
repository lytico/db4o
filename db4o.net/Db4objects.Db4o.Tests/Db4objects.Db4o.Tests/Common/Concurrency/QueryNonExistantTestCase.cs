/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class QueryNonExistantTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.QueryNonExistantTestCase().RunConcurrency
				();
		}

		internal QueryNonExistantTestCase.QueryNonExistant1 member;

		public QueryNonExistantTestCase()
		{
		}

		public QueryNonExistantTestCase(bool createMembers)
		{
			// db4o constructor
			member = new QueryNonExistantTestCase.QueryNonExistant1();
			member.member = new QueryNonExistantTestCase.QueryNonExistant2();
			member.member.member = this;
		}

		// db4o constructor
		public virtual void Conc(IExtObjectContainer oc)
		{
			oc.QueryByExample((new Db4objects.Db4o.Tests.Common.Concurrency.QueryNonExistantTestCase
				(true)));
			AssertOccurrences(oc, typeof(Db4objects.Db4o.Tests.Common.Concurrency.QueryNonExistantTestCase
				), 0);
			IQuery q = oc.Query();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Concurrency.QueryNonExistantTestCase
				(true));
			Assert.AreEqual(0, q.Execute().Count);
		}

		public class QueryNonExistant1
		{
			internal QueryNonExistantTestCase.QueryNonExistant2 member;
		}

		public class QueryNonExistant2 : QueryNonExistantTestCase.QueryNonExistant1
		{
			internal QueryNonExistantTestCase member;
		}
	}
}
#endif // !SILVERLIGHT
