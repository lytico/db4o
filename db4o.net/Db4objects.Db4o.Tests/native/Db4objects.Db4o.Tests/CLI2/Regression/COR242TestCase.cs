/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI2.Regression
{
#if !MONO
    using System.Collections.Generic;
    using Db4oUnit;
    using Db4oUnit.Extensions;

    public class COR242TestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public IList<string> items;

            public Item(IList<string> items_)
            {
                items = items_;
            }
        }

        protected override void Store()
        {
            Store(new Item(new string[] {"foo", "bar"}));
        }

        public void _Test()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof(Item));
            Assert.IsNotNull(item.items);
            Assert.IsInstanceOf(typeof(string[]), item.items);
            ArrayAssert.AreEqual(new string[] {"foo", "bar"}, (string[])item.items);
        }
    }
#endif
}
