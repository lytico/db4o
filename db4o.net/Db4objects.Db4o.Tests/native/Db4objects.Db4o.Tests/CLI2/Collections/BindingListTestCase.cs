#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    class BindingListTestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public BindingList<Element> _bindingList;
        }

        public class Element
        {
            public String _name;

            public Element(String name)
            {
                _name = name;
            }
        }

        protected override void Store()
        {
            Item item = new Item();
            item._bindingList = new BindingList<Element>();
            item._bindingList.Add(new Element("one"));
            Store(item);
        }

        public void TestRetrieve()
        {
            AssertSingleItem();
        }

        private void AssertSingleItem()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof (Item));
            Element element = item._bindingList[0];
            Assert.AreEqual("one", element._name);
        }

        public void TestDefragment()
        {
            Defragment();
            AssertSingleItem();
        }

    }
}
#endif