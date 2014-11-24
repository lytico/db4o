using System;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class EnumHandlerUpdateTestCase : HandlerUpdateTestCaseBase
    {
        enum EnumAsInteger
        {
            First = 1,
            Second,
            Third,
        }

        class Item
        {
            public EnumAsInteger _asInteger;

            public Item(EnumAsInteger asInteger)
            {
                _asInteger = asInteger;
            }

            public override bool Equals(object obj)
            {
                Item rhs = (Item)obj;
                if (rhs == null) return false;

                if (rhs.GetType() != GetType()) return false;

                return _asInteger == rhs._asInteger;
            }

            public override string ToString()
            {
                return "Item _asInteger:" + _asInteger ;
            }
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {

        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            Item item = (Item)values[0];
            Assert.AreEqual(EnumAsInteger.Second, item._asInteger);
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override object[] CreateValues()
        {
            Item[] values = new Item[1];
            Item item = new Item(EnumAsInteger.Second);
            values[0] = item;
            return values;
        }

        protected override string TypeName()
        {
            return "enum";
        }

		protected override void ConfigureForTest(Config.IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).ObjectField("_asInteger").Indexed(true);
		}

    }
}
