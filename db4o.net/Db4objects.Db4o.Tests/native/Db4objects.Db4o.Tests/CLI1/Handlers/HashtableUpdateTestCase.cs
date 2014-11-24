using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    public class HashtableUpdateTestCase : HandlerUpdateTestCaseBase
    {
        public class HashtableHolder
        {
            public IDictionary _dictionary;

            public Hashtable _hashtable;

            public object _untyped;
        }

        public class Item
        {
            public String _name;

            public Item(String name)
            {
                this._name = name;
            }


            public override int GetHashCode()
            {
                return _name != null ? _name.GetHashCode() : 0;
            }

            public override bool Equals(object obj)
            {
                if (! (obj is Item))
                {
                    return false;
                }
                Item other = (Item)obj;
                if (_name == null)
                {
                    return other._name == null;
                }
                return _name.Equals(other._name);
            }
           
        }

        protected override string TypeName()
        {
            return "Hashtable";
        }

        protected override object[] CreateValues()
        {
            HashtableHolder holder = new HashtableHolder();
            holder._dictionary = CreateHashtable();
            holder._hashtable = CreateHashtable();
            holder._untyped = CreateHashtable();
            return new object[]{holder};
        }

        private Hashtable CreateHashtable()
        {
            Hashtable ht = new Hashtable();
            ht[1] = 1;
            ht["string"] = "string";
            Item item = new Item("item");
            ht[item] = item;
            return ht;
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            HashtableHolder holder = (HashtableHolder) values[0];
            AssertHashtable(holder._dictionary);
            AssertHashtable(holder._hashtable);
            AssertHashtable(holder._untyped);
        }

        private void AssertHashtable(object obj)
        {
            Hashtable ht = obj as Hashtable;
            Assert.IsNotNull(ht);
            Assert.AreEqual(3, ht.Count);
            AssertHashtableEntry(ht, 1);
            AssertHashtableEntry(ht, "string");
            AssertHashtableEntry(ht, new Item("item"));
        }

        private void AssertHashtableEntry(Hashtable ht, object entry)
        {
            Assert.AreEqual(entry, ht[entry]);
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            // do nothing
        }
    }
}
