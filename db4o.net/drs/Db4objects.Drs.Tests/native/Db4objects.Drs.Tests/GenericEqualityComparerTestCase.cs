/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o;
using Db4oUnit;

namespace Db4objects.Drs.Tests
{
	class GenericEqualityComparerTestCase : DrsTestCase
	{
		public class Item
		{
			public IEqualityComparer<string> comparer;

			public Item(IEqualityComparer<string> comparer)
			{
				this.comparer = comparer;
			}
		}

		public void Test()
		{
			A().Provider().StoreNew(new Item(EqualityComparer<string>.Default));
			A().Provider().Commit();

			ReplicateAll(A().Provider(), B().Provider());

			IObjectSet found = B().Provider().GetStoredObjects(typeof(Item));
			Assert.AreEqual(1, found.Count);

			Item item = (Item) found[0];
			Assert.IsNotNull(item.comparer);
		}
	}
}
