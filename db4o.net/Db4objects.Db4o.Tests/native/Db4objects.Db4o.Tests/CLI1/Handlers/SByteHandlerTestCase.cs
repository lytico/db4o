/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    public class SByteHandlerTestCase : TypeHandlerTestCaseBase
    {
		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(false);
		}

		public virtual void TestReadWrite()
        {
            MockWriteContext writeContext = new MockWriteContext(Db());
            sbyte expected = 0x11;
            SByteHandler().Write(writeContext, expected);
            MockReadContext readContext = new MockReadContext(writeContext);
            sbyte sbyteValue = (sbyte)SByteHandler().Read(readContext);
            Assert.AreEqual(expected, sbyteValue);
        }

        public virtual void TestStoreObject()
        {
            Item storedItem = new Item(0x11, 0x22);
            DoTestStoreObject(storedItem);
        }

        private Db4o.Internal.Handlers.SByteHandler SByteHandler()
        {
            return new Db4o.Internal.Handlers.SByteHandler();
        }

        public class Item
        {
            public sbyte _sbyte;

            public SByte _sbyteWrapper;

            public Item(sbyte s, SByte wrapper)
            {
                _sbyte = s;
                _sbyteWrapper = wrapper;
            }

            public override bool Equals(object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                if (!(obj is Item))
                {
                    return false;
                }
                Item other = (Item)obj;
                return (other._sbyte == _sbyte) && _sbyteWrapper.Equals(other._sbyteWrapper
                    );
            }

            public override string ToString()
            {
                return "[" + _sbyte + "," + _sbyteWrapper + "]";
            }
        }

    }
}
