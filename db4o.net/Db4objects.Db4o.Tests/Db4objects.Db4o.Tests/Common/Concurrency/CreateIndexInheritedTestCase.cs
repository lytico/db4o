/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CreateIndexInheritedTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.CreateIndexInheritedTestCase().RunConcurrencyAll
				();
		}

		public int i_int;

		public CreateIndexInheritedTestCase()
		{
		}

		public CreateIndexInheritedTestCase(int a_int)
		{
			i_int = a_int;
		}

		protected override void Store()
		{
			Store(new CreateIndexInheritedTestCase.CreateIndexFor("a"));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor("c"));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor("b"));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor("f"));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor("e"));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(1));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(5));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(7));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(3));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(2));
			Store(new CreateIndexInheritedTestCase.CreateIndexFor(3));
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Db4objects.Db4o.Tests.Common.Concurrency.CreateIndexInheritedTestCase
				)).ObjectField("i_int").Indexed(true);
			config.ObjectClass(typeof(CreateIndexInheritedTestCase.CreateIndexFor)).ObjectField
				("i_name").Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc1(IExtObjectContainer oc)
		{
			TQueryB(oc);
			TQueryInts(oc, 5);
		}

		public virtual void Conc2(IExtObjectContainer oc)
		{
			oc.Store(new CreateIndexInheritedTestCase.CreateIndexFor("d"));
			TQueryB(oc);
			TUpdateB(oc);
			oc.Store(new CreateIndexInheritedTestCase.CreateIndexFor("z"));
			oc.Store(new CreateIndexInheritedTestCase.CreateIndexFor("y"));
		}

		public virtual void Check2(IExtObjectContainer oc)
		{
			TQueryB(oc);
			TQueryInts(oc, 5 + ThreadCount() * 3);
		}

		private void TQueryInts(IExtObjectContainer oc, int expectedZeroSize)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(0);
			int zeroSize = q.Execute().Count;
			Assert.AreEqual(expectedZeroSize, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(4).Greater().Equal();
			TExpectInts(q, new int[] { 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(4).Greater();
			TExpectInts(q, new int[] { 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(3).Greater();
			TExpectInts(q, new int[] { 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(3).Greater().Equal();
			TExpectInts(q, new int[] { 3, 3, 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(2).Greater().Equal();
			TExpectInts(q, new int[] { 2, 3, 3, 5, 7 });
			q = oc.Query();
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(2).Greater();
			TExpectInts(q, new int[] { 3, 3, 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(1).Greater().Equal();
			TExpectInts(q, new int[] { 1, 2, 3, 3, 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(1).Greater();
			TExpectInts(q, new int[] { 2, 3, 3, 5, 7 });
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(4).Smaller();
			TExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(4).Smaller().Equal();
			TExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(3).Smaller();
			TExpectInts(q, new int[] { 1, 2 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(3).Smaller().Equal();
			TExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(2).Smaller().Equal();
			TExpectInts(q, new int[] { 1, 2 }, zeroSize);
			q = oc.Query();
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(2).Smaller();
			TExpectInts(q, new int[] { 1 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(1).Smaller().Equal();
			TExpectInts(q, new int[] { 1 }, zeroSize);
			q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_int").Constrain(1).Smaller();
			TExpectInts(q, new int[] {  }, zeroSize);
		}

		private void TExpectInts(IQuery q, int[] ints, int zeroSize)
		{
			IObjectSet res = q.Execute();
			Assert.AreEqual(ints.Length + zeroSize, res.Count);
			while (res.HasNext())
			{
				CreateIndexInheritedTestCase.CreateIndexFor ci = (CreateIndexInheritedTestCase.CreateIndexFor
					)res.Next();
				for (int i = 0; i < ints.Length; i++)
				{
					if (ints[i] == ci.i_int)
					{
						ints[i] = 0;
						break;
					}
				}
			}
			for (int i = 0; i < ints.Length; i++)
			{
				Assert.AreEqual(0, ints[i]);
			}
		}

		private void TExpectInts(IQuery q, int[] ints)
		{
			TExpectInts(q, ints, 0);
		}

		private void TQueryB(IExtObjectContainer oc)
		{
			IObjectSet res = Query(oc, "b");
			Assert.AreEqual(1, res.Count);
			CreateIndexInheritedTestCase.CreateIndexFor ci = (CreateIndexInheritedTestCase.CreateIndexFor
				)res.Next();
			Assert.AreEqual("b", ci.i_name);
		}

		private void TUpdateB(IExtObjectContainer oc)
		{
			IObjectSet res = Query(oc, "b");
			CreateIndexInheritedTestCase.CreateIndexFor ci = (CreateIndexInheritedTestCase.CreateIndexFor
				)res.Next();
			ci.i_name = "j";
			oc.Store(ci);
			res = Query(oc, "b");
			Assert.AreEqual(0, res.Count);
			res = Query(oc, "j");
			Assert.AreEqual(1, res.Count);
			ci.i_name = "b";
			oc.Store(ci);
			TQueryB(oc);
		}

		private IObjectSet Query(IExtObjectContainer oc, string n)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(CreateIndexInheritedTestCase.CreateIndexFor));
			q.Descend("i_name").Constrain(n);
			return q.Execute();
		}

		public class CreateIndexFor : CreateIndexInheritedTestCase
		{
			public string i_name;

			public CreateIndexFor()
			{
			}

			public CreateIndexFor(string name)
			{
				this.i_name = name;
			}

			public CreateIndexFor(int a_int) : base(a_int)
			{
			}
		}
	}
}
#endif // !SILVERLIGHT
