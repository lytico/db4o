/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4oUnit.Extensions;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
    public class ListOfNullableItemTestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public IList<int?> nullableList;
            public Item(IList<int?> nullableList_)
            {
                nullableList = nullableList_;
            }

        }

        private static IList<int?> nullableIntList1()
        {
            return new List<int?>(new int?[] { 1, 2, 3 });
        }

        protected override void Store()
        {
            Item item = new Item(nullableIntList1());
            Store(item);
        }

        public void test() 
        {
            Item item = (Item)RetrieveOnlyInstance(typeof(Item));
            Assert.IsNotNull(item.nullableList);
            Iterator4Assert.AreEqual(nullableIntList1().GetEnumerator(), item.nullableList.GetEnumerator());
        }
    }
}
