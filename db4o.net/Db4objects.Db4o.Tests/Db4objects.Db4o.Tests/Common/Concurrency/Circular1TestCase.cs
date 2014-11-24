/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class Circular1TestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Circular1TestCase().RunConcurrency();
		}

		protected override void Store()
		{
			Store(new Circular1TestCase.C1C());
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(Circular1TestCase.C1C), 1);
		}

		public class C1P
		{
			internal Circular1TestCase.C1C c;
		}

		public class C1C : Circular1TestCase.C1P
		{
		}
	}
}
#endif // !SILVERLIGHT
