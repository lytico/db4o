/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	public abstract partial class AbstractActivatableCollectionApiTestCase<TColl, TElem> : AbstractDb4oTestCase where TColl : class,ICollection<TElem>
	{
		protected static readonly IList<string> Names = new List<string>(new string[] {"one", "two", "three", "four"});

		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
		}

		protected override void Store()
		{
			CollectionHolder<TColl> item = new CollectionHolder<TColl>(NewPopulatedActivatableCollection());
			Store(item);
		}

		public void TestIteratorOnEmptyCollection()
		{
			foreach (var item in NewActivatableCollection())
			{
			}
		}

		public void TestAdd()
		{
			AssertCollectionChange(delegate(TColl list)
			{
				list.Add( NewElement("five") );
			});
		}

		public void TestClear()
		{
			SingleCollection().Clear();
			Reopen();
			
			TColl expected = NewPopulatedPlainCollection();
			Assert.IsGreater(0, expected.Count);
			expected.Clear();
			IteratorAssert.SameContent(expected, SingleCollection());
		}

		public void TestContains()
		{
			Assert.IsTrue(SingleCollection().Contains( NewElement("one") ));
			Assert.IsFalse(SingleCollection().Contains( NewElement("five") ));
		}

		public void TestCopyTo()
		{
			TColl plainCollection = NewPopulatedPlainCollection();
			TElem[] target = new TElem[plainCollection.Count];

			TColl list = SingleCollection();
			list.CopyTo(target, 0);
			Assert.IsTrue(Db().IsActive(list));
			IteratorAssert.AreEqual(list.GetEnumerator(), target.GetEnumerator());
			IteratorAssert.AreEqual(plainCollection.GetEnumerator(), target.GetEnumerator());
		}

		public void TestIsReadOnly()
		{
			Assert.IsFalse(SingleCollection().IsReadOnly);
		}

		public void TestRemove()
		{
			AssertCollectionChange(delegate(TColl list)
			{
				list.Remove( NewElement("one") );
			});
		}

		public void TestCount()
		{
			Assert.AreEqual(NewPopulatedPlainCollection().Count, SingleCollection().Count);
		}

		protected int LastIndex()
		{
			return Names.Count * 2 - 1;
		}

		protected abstract TColl NewPlainCollection();
		protected abstract TColl SingleCollection();
		protected abstract TColl NewActivatableCollection(TColl template);
		protected abstract TColl NewActivatableCollection();
		
		protected abstract TElem NewElement(string value);
		protected abstract TElem NewActivatableElement(string value);
	}
}
