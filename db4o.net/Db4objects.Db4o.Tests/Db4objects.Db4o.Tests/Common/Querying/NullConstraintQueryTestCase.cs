/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class NullConstraintQueryTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new NullConstraintQueryTestCase().RunAll();
		}

		private sealed class LoadedFromClassIndexListener : IDiagnosticListener
		{
			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is LoadedFromClassIndex)
				{
					Assert.Fail("Query should not be loaded from class index");
				}
			}

			internal LoadedFromClassIndexListener(NullConstraintQueryTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly NullConstraintQueryTestCase _enclosing;
		}

		public class ObjectItem
		{
			public string _name;

			public NullConstraintQueryTestCase.ObjectItem _child;

			public ObjectItem(NullConstraintQueryTestCase.ObjectItem child, string name)
			{
				_child = child;
				_name = name;
			}
		}

		public class StringItem
		{
			public string _name;

			public StringItem(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(new NullConstraintQueryTestCase.LoadedFromClassIndexListener
				(this));
			config.ObjectClass(typeof(NullConstraintQueryTestCase.ObjectItem)).ObjectField("_child"
				).Indexed(true);
			config.ObjectClass(typeof(NullConstraintQueryTestCase.StringItem)).ObjectField("_name"
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			NullConstraintQueryTestCase.ObjectItem childItem = new NullConstraintQueryTestCase.ObjectItem
				(null, "child");
			NullConstraintQueryTestCase.ObjectItem parentItem = new NullConstraintQueryTestCase.ObjectItem
				(childItem, "parent");
			Store(parentItem);
			Store(new NullConstraintQueryTestCase.StringItem(null));
			Store(new NullConstraintQueryTestCase.StringItem(null));
			Store(new NullConstraintQueryTestCase.StringItem("one"));
			Store(new NullConstraintQueryTestCase.StringItem("two"));
		}

		public virtual void TestQueryForNullChild()
		{
			IQuery q = NewQuery(typeof(NullConstraintQueryTestCase.ObjectItem));
			q.Descend("_child").Constrain(null);
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			NullConstraintQueryTestCase.ObjectItem item = ((NullConstraintQueryTestCase.ObjectItem
				)objectSet.Next());
			Assert.AreEqual("child", item._name);
		}

		public virtual void TestQueryForNullString()
		{
			IQuery q = NewQuery(typeof(NullConstraintQueryTestCase.StringItem));
			q.Descend("_name").Constrain(null);
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(2, objectSet.Count);
			NullConstraintQueryTestCase.StringItem item = ((NullConstraintQueryTestCase.StringItem
				)objectSet.Next());
			Assert.IsNull(item._name);
			item = ((NullConstraintQueryTestCase.StringItem)objectSet.Next());
			Assert.IsNull(item._name);
		}
	}
}
