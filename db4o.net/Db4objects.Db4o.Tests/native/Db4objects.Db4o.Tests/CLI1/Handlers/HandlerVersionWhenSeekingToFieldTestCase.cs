/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class HandlerVersionWhenSeekingToFieldTestCase : FormatMigrationTestCaseBase
	{
		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer)
		{
			var result = QuerySpecificObjects(objectContainer);
			
			Assert.AreEqual(1, result.Count);

			var item = (Item) result[0];
			Assert.AreEqual(NewItem(1971, 5, 1), item);
		}

		protected override void AssertObjectsAreUpdated(IExtObjectContainer objectContainer)
		{
			var result = QuerySpecificObjects(objectContainer);
			Assert.AreEqual(2, result.Count);
		}

		protected override void Update(IExtObjectContainer objectContainer)
		{
			objectContainer.Store(NewItem(2004, 2, 23));
			objectContainer.Commit();
		}

		private static IObjectSet QuerySpecificObjects(IExtObjectContainer objectContainer)
		{
			var query = objectContainer.Query();
			query.Constrain(typeof (Item));
			query.Descend("value").Constrain(42);

			return query.Execute();
		}

		protected override string FileNamePrefix()
		{
			return "SeekToField";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			objectContainer.Store(NewItem(1971, 5, 1));
		}

		private static Item NewItem(int year, int month, int day)
		{
			return new Item(new DateTime(year, month, day), 42);
		}
	}

	public class Item
	{
		private DateTime date;
		private int value;

		public Item(DateTime date, int value)
		{
			this.date = date;
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof(Item)) return false;

			Item other = (Item) obj;
			return other.date == date && other.value == value;
		}
	}
}
