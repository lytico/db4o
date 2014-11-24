using System;
using System.Collections.Generic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre12.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
#if NET_3_5 && ! CF
    internal class HashSetTestCase : AbstractDb4oTestCase
    {
        private static string[] STRING_DATA = new string[] {"one", "two", "three"};

        private static Element[] ELEMENT_DATA = new Element[]{
            new Element("one"),
            new Element("two"),
            new Element("three")
        } ;

        private static int[] INT_DATA = new int[] {1,2,3};


        public class Item
        {
            public HashSet<string> _strings;

            public HashSet<Element> _elements;

            public HashSet<int> _ints;

        }

        public class Element
        {
            public string _name;

            public Element(string name)
            {
                _name = name;
            }

            public override bool Equals(object obj)
            {
                Element other = obj as Element;
                if(other == null)
                {
                    return false;
                }
                if(_name == null)
                {
                    return other._name == null;
                }
                return _name.Equals(other._name);
            }

            public override int GetHashCode()
            {
                if(_name == null)
                {
                    return 0;
                }
                return _name.GetHashCode();
            }
        }


        protected override void Store()
        {
            Item item = new Item();
            item._strings = new HashSet<string>();
            AddAll(item._strings, STRING_DATA);
            item._elements = new HashSet<Element>();
            AddAll(item._elements, ELEMENT_DATA);
            item._ints = new HashSet<int>();
            AddAll(item._ints, INT_DATA);
            Store(item);
        }

        private void AddAll<T>(HashSet<T> hashSet, Array array)
        {
            foreach (T o in array)
            {
                hashSet.Add(o);
            }
        }

        public void Test()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof (Item));
            AssertItem(item);
        }

        private void AssertItem(Item item)
        {
            IteratorAssert.AreEqual(STRING_DATA.GetEnumerator(), item._strings.GetEnumerator());
            IteratorAssert.AreEqual(ELEMENT_DATA.GetEnumerator(), item._elements.GetEnumerator());
            IteratorAssert.AreEqual(INT_DATA.GetEnumerator(), item._ints.GetEnumerator());
        }

        public void TestDeleteRollback()
        {
            Item item = (Item)RetrieveOnlyInstance(typeof(Item));
            Db().Delete(item._elements);
            Db().Delete(item._ints);
            Db().Delete(item._strings);
            Db().Refresh(item, int.MaxValue);
            item._elements = null;
            item._ints = null;
            item._strings = null;
            Db().Rollback();
            Db().Refresh(item, int.MaxValue);
            AssertItem(item);
        }

        public void TestQuery()
        {
            IQuery q = Db().Query();
            q.Constrain(typeof (Item));
            q.Descend(("_strings")).Constrain("one");
            IObjectSet objectSet = q.Execute();
            Item item = (Item) objectSet.Next();
            AssertItem(item);
        }
    }
#endif 
}
