/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	/// <exclude></exclude>
	public class DoubleFieldIndexTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new DoubleFieldIndexTestCase().RunSolo();
		}

		public class Item
		{
			public double value;

			public Item()
			{
			}

			public Item(double value_)
			{
				value = value_;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			IndexField(config, typeof(DoubleFieldIndexTestCase.Item), "value");
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new DoubleFieldIndexTestCase.Item(0.5));
			Db().Store(new DoubleFieldIndexTestCase.Item(1.1));
			Db().Store(new DoubleFieldIndexTestCase.Item(2));
		}

		public virtual void TestEqual()
		{
			IQuery query = NewQuery(typeof(DoubleFieldIndexTestCase.Item));
			query.Descend("value").Constrain(1.1);
			AssertItems(new double[] { 1.1 }, query.Execute());
		}

		public virtual void TestGreater()
		{
			IQuery query = NewQuery(typeof(DoubleFieldIndexTestCase.Item));
			IQuery descend = query.Descend("value");
			descend.Constrain(System.Convert.ToDouble(1)).Greater();
			descend.OrderAscending();
			AssertItems(new double[] { 1.1, 2 }, query.Execute());
		}

		private void AssertItems(double[] expected, IObjectSet set)
		{
			ArrayAssert.AreEqual(expected, ToDoubleArray(set));
		}

		private double[] ToDoubleArray(IObjectSet set)
		{
			double[] array = new double[set.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((DoubleFieldIndexTestCase.Item)set.Next()).value;
			}
			return array;
		}
	}
}
