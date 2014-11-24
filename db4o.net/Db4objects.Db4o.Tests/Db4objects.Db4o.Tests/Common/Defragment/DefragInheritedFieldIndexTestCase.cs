/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class DefragInheritedFieldIndexTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		private static readonly string FieldName = "_name";

		private static readonly string[] Names = new string[] { "Foo", "Bar", "Baz" };

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(DefragInheritedFieldIndexTestCase.ParentItem)).ObjectField
				(FieldName).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int nameIndex = 0; nameIndex < Names.Length; ++nameIndex)
			{
				string name = Names[nameIndex];
				Store(new DefragInheritedFieldIndexTestCase.ChildItem(name));
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDefragInheritedFieldIndex()
		{
			AssertQueryByIndex();
			Defragment();
			AssertQueryByIndex();
		}

		private void AssertQueryByIndex()
		{
			IQuery query = NewQuery(typeof(DefragInheritedFieldIndexTestCase.ChildItem));
			query.Descend(FieldName).Constrain(Names[0]);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(Names[0], ((DefragInheritedFieldIndexTestCase.ChildItem)result.Next
				())._name);
		}

		public class ParentItem
		{
			public string _name;

			public ParentItem(string name)
			{
				_name = name;
			}
		}

		public class ChildItem : DefragInheritedFieldIndexTestCase.ParentItem
		{
			public ChildItem(string name) : base(name)
			{
			}
		}
	}
}
