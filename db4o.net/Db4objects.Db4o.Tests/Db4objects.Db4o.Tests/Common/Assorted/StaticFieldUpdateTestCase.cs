/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class StaticFieldUpdateTestCase : AbstractDb4oTestCase
	{
		public class SimpleEnum
		{
			public string _name;

			public SimpleEnum(string name)
			{
				_name = name;
			}

			public static StaticFieldUpdateTestCase.SimpleEnum A = new StaticFieldUpdateTestCase.SimpleEnum
				("A");

			public static StaticFieldUpdateTestCase.SimpleEnum B = new StaticFieldUpdateTestCase.SimpleEnum
				("B");
		}

		public class Item
		{
			public StaticFieldUpdateTestCase.SimpleEnum _value;

			public Item(StaticFieldUpdateTestCase.SimpleEnum value)
			{
				_value = value;
			}
		}

		private const int NumItems = 100;

		private const int NumRuns = 10;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.UpdateDepth(5);
			config.ObjectClass(typeof(StaticFieldUpdateTestCase.SimpleEnum)).PersistStaticFieldValues
				();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(StaticFieldUpdateTestCase.SimpleEnum.A, NumItems);
			Store(StaticFieldUpdateTestCase.SimpleEnum.B, NumItems);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			for (int runIdx = 0; runIdx < NumRuns; runIdx++)
			{
				UpdateAll();
				AssertCount(StaticFieldUpdateTestCase.SimpleEnum.A, NumItems);
				AssertCount(StaticFieldUpdateTestCase.SimpleEnum.B, NumItems);
				Reopen();
			}
		}

		private void Store(StaticFieldUpdateTestCase.SimpleEnum value, int count)
		{
			for (int idx = 0; idx < count; idx++)
			{
				Store(new StaticFieldUpdateTestCase.Item(value));
			}
		}

		private void UpdateAll()
		{
			IObjectSet result = NewQuery(typeof(StaticFieldUpdateTestCase.Item)).Execute();
			while (result.HasNext())
			{
				StaticFieldUpdateTestCase.Item item = ((StaticFieldUpdateTestCase.Item)result.Next
					());
				item._value = (item._value == StaticFieldUpdateTestCase.SimpleEnum.A) ? StaticFieldUpdateTestCase.SimpleEnum
					.B : StaticFieldUpdateTestCase.SimpleEnum.A;
				Store(item);
			}
			Commit();
		}

		private void AssertCount(StaticFieldUpdateTestCase.SimpleEnum value, int count)
		{
			ConsistencyReport consistencyReport = new ConsistencyChecker(FileSession()).CheckSlotConsistency
				();
			if (!consistencyReport.Consistent())
			{
				Sharpen.Runtime.Err.WriteLine(consistencyReport);
				throw new InvalidOperationException("Inconsistency found");
			}
			IQuery query = NewQuery(typeof(StaticFieldUpdateTestCase.Item));
			query.Descend("_value").Constrain(value);
			IObjectSet result = query.Execute();
			Assert.AreEqual(count, result.Count);
			while (result.HasNext())
			{
				Assert.AreEqual(value, ((StaticFieldUpdateTestCase.Item)result.Next())._value);
			}
		}
	}
}
