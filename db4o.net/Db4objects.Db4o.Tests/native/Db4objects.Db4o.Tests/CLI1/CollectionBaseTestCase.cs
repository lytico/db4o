#if !SILVERLIGHT
using System;
using System.Collections;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    class CollectionBaseTestCase : AbstractDb4oTestCase
    {
        private static string[] DATA = new string[] {"one", "two", "three"};

        public class Item
        {
            public String _name;

            public CustomCollection _collection;
        }

        public class CustomCollection : CollectionBase
        {
            public void Add(object obj)
            {
                List.Add(obj);
            }

            public object this[int index]
            {
                get
                {
                    return List[index];
                }
            }
        }

        protected override void Store()
        {
            foreach (String str in DATA)
            {
                Item item = new Item();
                item._name = str;
                item._collection = new CustomCollection();
                item._collection.Add(str);
                Store(item);
            }
        }

        public void TestRetrieve()
        {
            IQuery q = Db().Query();
            q.Constrain(typeof(Item));
            IObjectSet result = q.Execute();
            Assert.AreEqual(DATA.Length, result.Count);
            foreach(Item item in result)
            {
                Assert.AreEqual(item._name, item._collection[0]);
            }
        }

        public void TestQuery()
        {
            foreach(String str in DATA)
            {
                IQuery q = Db().Query();
                q.Constrain(typeof(Item));
                q.Descend("_collection").Constrain(str);
                IObjectSet result = q.Execute();
                Assert.AreEqual(1, result.Count);
                Item item = (Item) result[0];
                Assert.AreEqual(str, item._name);
                Assert.AreEqual(str, item._collection[0]);
            }
        }

    }
}
#endif