/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class DeepSetTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new DeepSetTestCase().RunConcurrency();
		}

		public DeepSetTestCase child;

		public string name;

		protected override void Store()
		{
			name = "1";
			child = new DeepSetTestCase();
			child.name = "2";
			child.child = new DeepSetTestCase();
			child.child.name = "3";
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc, int seq)
		{
			DeepSetTestCase example = new DeepSetTestCase();
			example.name = "1";
			DeepSetTestCase ds = (DeepSetTestCase)oc.QueryByExample(example).Next();
			Assert.AreEqual("1", ds.name);
			Assert.AreEqual("3", ds.child.child.name);
			ds.name = "1";
			ds.child.name = "12" + seq;
			ds.child.child.name = "13" + seq;
			oc.Store(ds, 2);
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			DeepSetTestCase example = new DeepSetTestCase();
			example.name = "1";
			DeepSetTestCase ds = (DeepSetTestCase)oc.QueryByExample(example).Next();
			Assert.IsTrue(ds.child.name.StartsWith("12"));
			Assert.IsTrue(ds.child.name.Length > "12".Length);
			Assert.AreEqual("3", ds.child.child.name);
		}
	}
}
#endif // !SILVERLIGHT
