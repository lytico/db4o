/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    /// <summary>
    /// Summary description for CsMarshalByRef.
    /// </summary>
    public class CsMarshalByRef : AbstractDb4oTestCase
    {
        public class Item : System.MarshalByRefObject
        {
            public int _placeHolder;
            public string _field;
        }

        override protected void Store()
        {
            Item item = new Item();
            item._field = "foo";
            item._placeHolder = 42;
            Store(item);
        }

        public void Test()
        {
            Item item = (Item)RetrieveOnlyInstance(typeof(Item));
            Assert.AreEqual("foo", item._field);
            Assert.AreEqual(42, item._placeHolder);
        }
    }
}
#endif