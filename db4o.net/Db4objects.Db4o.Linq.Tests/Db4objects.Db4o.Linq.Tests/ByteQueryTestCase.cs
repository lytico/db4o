using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
    public class ByteQueryTestCase : AbstractDb4oLinqTestCase
    {
        public class Item
        {
            public byte b;
            public string name;
            public string pass;
        }

        protected override void Store()
        {
            Store(new Item { b = 0, name = "foo", pass = "foo" });
            Store(new Item { b = 1, name = "bar", pass = "bar" });
        }

        public void TestQuery()
        {
            IEnumerable<Item> result = from Item item in Db()
                                       where item.b == (byte)0 && item.name == "foo" && item.pass == "foo"
                                       select item;
            Assert.AreEqual(1, result.Count());
        }
    }
}
