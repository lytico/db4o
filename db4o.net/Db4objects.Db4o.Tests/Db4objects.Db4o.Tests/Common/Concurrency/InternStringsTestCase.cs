/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class InternStringsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase().RunConcurrency
				();
		}

		public string _name;

		public InternStringsTestCase() : this(null)
		{
		}

		public InternStringsTestCase(string name)
		{
			_name = name;
		}

		protected override void Configure(IConfiguration config)
		{
			config.InternStrings(true);
		}

		protected override void Store()
		{
			string name = "Foo";
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase(name));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase(name));
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery query = oc.Query();
			query.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				));
			IObjectSet result = query.Execute();
			Assert.AreEqual(2, result.Count);
			Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase first = (Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				)result.Next();
			Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase second = (Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				)result.Next();
			Assert.AreSame(first._name, second._name);
		}
	}
}
#endif // !SILVERLIGHT
