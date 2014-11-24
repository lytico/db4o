/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class NonSerializedAttributeTestCase : AbstractDb4oTestCase
    {
		public struct Pair
		{
			public Pair(string name, int value)
			{
				Value = value;
				Name = name;
			}

			public override string ToString()
			{
				return string.Format("Pair({0}, {1}", Name, Value);
			}

			public int Value;
			public string Name;
		}

        public class Item
        {
            [NonSerialized]
            public int NonSerializedValue;

            [Transient]
            public int TransientValue;

        	[Transient]
			public Pair Pair;

            public int Value;
            
            public Item()
            {   
            }
            
            public Item(int value)
            {
                Value = value;
                NonSerializedValue = value;
                TransientValue = value;
				Pair = new Pair("p1", value);
            }
        }
        
        public class DerivedItem : Item
        {
            public DerivedItem()
            {   
            }
            
            public DerivedItem(int value) : base(value)
            {
            }
        }

        protected override void Store()
        {
            Store(new Item(42));
            Store(new DerivedItem(42));
        }
        
        public void Test()
        {
            IObjectSet found = NewQuery(typeof(Item)).Execute();
            Assert.AreEqual(2, found.Count);
            foreach (Item item in found)
            {
                Assert.AreEqual(0, item.NonSerializedValue);
                Assert.AreEqual(0, item.TransientValue);
				Assert.AreEqual(new Pair(), item.Pair);
                Assert.AreEqual(42, item.Value);
            }
        }
    }
}
