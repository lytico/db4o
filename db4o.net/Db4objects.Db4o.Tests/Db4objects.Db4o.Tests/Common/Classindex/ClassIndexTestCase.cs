/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Tests.Common.Classindex;

namespace Db4objects.Db4o.Tests.Common.Classindex
{
	public class ClassIndexTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public class Item
		{
			public string name;

			public Item(string _name)
			{
				this.name = _name;
			}
		}

		public static void Main(string[] args)
		{
			new ClassIndexTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDelete()
		{
			ClassIndexTestCase.Item item = new ClassIndexTestCase.Item("test");
			Store(item);
			int id = (int)Db().GetID(item);
			AssertID(id);
			Reopen();
			item = (ClassIndexTestCase.Item)Db().QueryByExample(item).Next();
			id = (int)Db().GetID(item);
			AssertID(id);
			Db().Delete(item);
			Db().Commit();
			AssertEmpty();
			Reopen();
			AssertEmpty();
		}

		private void AssertID(int id)
		{
			AssertIndex(new object[] { id });
		}

		private void AssertEmpty()
		{
			AssertIndex(new object[] {  });
		}

		private void AssertIndex(object[] expected)
		{
			ExpectingVisitor visitor = new ExpectingVisitor(expected);
			IClassIndexStrategy index = ClassMetadataFor(typeof(ClassIndexTestCase.Item)).Index
				();
			index.TraverseIds(Trans(), visitor);
			visitor.AssertExpectations();
		}
	}
}
