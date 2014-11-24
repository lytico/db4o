/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    /// <summary>
    /// testing deactivation and zero date.
    /// </summary>
    public class CsDate : AbstractDb4oTestCase
    {
        public class Item
        {
            public DateTime dateTime;

            public bool flag = true;

            public Item()
            {
            }

            public Item(DateTime value)
            {
                dateTime = value;
            }
        }

        override protected void Store()
        {
            Store(new Item(DateTime.Now));
        }

        public void TestTrivialQuery()
        {
            IQuery q = NewQuery(typeof(Item));
            IObjectSet os = q.Execute();
            Assert.AreEqual(1, os.Count);
        }

        public void TestQueryByExample()
        {
            Item template = new Item();
            GetOne(template);

            template.dateTime = new DateTime(0);
            GetOne(template);

            template.dateTime = new DateTime(100);
            Assert.AreEqual(0, Db().QueryByExample(template).Count);
        }

        private void GetOne(object template)
        {
			Assert.AreEqual(1, Db().QueryByExample(template).Count);
        }

        public void TestDeactivation()
        {
        	Item item = RetrieveOnlyInstance<Item>();
        	Db().Deactivate(item, int.MaxValue);
            Assert.AreEqual(new DateTime(0), item.dateTime);
        }

        public void TestSODA()
        {
            DateTime before = DateTime.Now.AddDays(-1);
            DateTime after = DateTime.Now.AddDays(1);

            IQuery q = NewQuery(typeof(Item));
            q.Descend("dateTime").Constrain(before).Smaller();
            Assert.AreEqual(0, q.Execute().Count);

            q = NewQuery(typeof(Item));
            q.Descend("dateTime").Constrain(after).Greater();
            Assert.AreEqual(0, q.Execute().Count);

            q = NewQuery(typeof(Item));
            q.Descend("dateTime").Constrain(before).Greater();
            Assert.AreEqual(1, q.Execute().Count);

            q = NewQuery(typeof(Item));
            q.Descend("dateTime").Constrain(after).Smaller();
            Assert.AreEqual(1, q.Execute().Count);

            q = NewQuery(typeof(Item));
            q.Descend("flag").Constrain(true);
            q.Descend("dateTime").Constrain(before).Greater();
            q.Descend("dateTime").Constrain(after).Smaller();
            Assert.AreEqual(1, q.Execute().Count);

            q = NewQuery(typeof(Item));
            q.Descend("flag").Constrain(false);
            q.Descend("dateTime").Constrain(before).Greater();
            q.Descend("dateTime").Constrain(after).Smaller();
            Assert.AreEqual(0, q.Execute().Count);
        }
    }
}
