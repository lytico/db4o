/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class GetAllTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new GetAllTestCase().RunConcurrency();
		}

		protected override void Store()
		{
			Store(new GetAllTestCase());
			Store(new GetAllTestCase());
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			Assert.AreEqual(2, oc.QueryByExample(null).Count);
		}

		public virtual void ConcSODA(IExtObjectContainer oc)
		{
			Assert.AreEqual(2, oc.Query().Execute().Count);
		}
	}
}
#endif // !SILVERLIGHT
