/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class EnumTestCase : AbstractDb4oTestCase
    {
        public enum MyEnum { A, B, C, D, F, INCOMPLETE }; 

        public class Item 
		{ 
            public MyEnum _enum; 
        } 

       protected override void Store()
       {
           Item item = new Item();
           item._enum = MyEnum.C;
           Store(item);
       }

       public void TestRetrieve()
       {
           Item item = (Item)RetrieveOnlyInstance(typeof(Item));
           Assert.AreEqual(MyEnum.C, item._enum);
       }

       public void TestPeekPersisted()
       {
           Item item = (Item) RetrieveOnlyInstance(typeof (Item));
           Item peeked = (Item) Db().PeekPersisted(item, int.MaxValue, true);
           Assert.AreEqual(item._enum, peeked._enum);
       }

    } 

}
