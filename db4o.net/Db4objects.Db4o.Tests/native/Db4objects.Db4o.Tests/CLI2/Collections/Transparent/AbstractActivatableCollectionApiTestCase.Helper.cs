/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	abstract partial class AbstractActivatableCollectionApiTestCase<TColl, TElem>
	{
		protected void AssertCopy(Action<TElem[]> copyAction)
		{
			TElem[] elements = new TElem[NewPopulatedPlainCollection().Count];
			copyAction(elements);

			foreach (string name in Names)
			{
				Assert.IsGreaterOrEqual(0, Array.IndexOf<TElem>(elements, NewElement(name) ));
				Assert.IsGreaterOrEqual(0, Array.IndexOf<TElem>(elements, NewActivatableElement(name) ));
			}
		}

		protected void AssertCollectionChange(Action<TColl> action)
		{
			action(SingleCollection());
			Reopen();

			TColl expected = NewPopulatedPlainCollection();
			action(expected);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleCollection().GetEnumerator());
		}

		private TColl NewPopulatedActivatableCollection()
		{
			return NewActivatableCollection(NewPopulatedPlainCollection());
		}
		
		protected TColl NewPopulatedPlainCollection()
		{
			TColl coll = NewPlainCollection();
			return PopulateNewCollection(coll);
		}

		private TColl PopulateNewCollection(TColl coll)
		{
			foreach (string name in Names)
			{
				coll.Add(NewElement(name));
			}

			foreach (string name in Names)
			{
				coll.Add(NewActivatableElement(name));
			}

			return coll;
		}

	}
}
