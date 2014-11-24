using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class SByteHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        public class Item
        {
            public sbyte _typedPrimitive;

            public object _untyped;

			public sbyte? _nullablePrimitive;
        }

        public class ItemArrays
        {
            public sbyte[] _typedPrimitiveArray;

            public object _primitiveArrayInObject;

			public sbyte?[] _nullableTypedPrimitiveArray;
        }

        private static readonly sbyte[] data = new sbyte[] {
            sbyte.MinValue,
            sbyte.MinValue + 1,
            -5,
            -1,
            0,
            1,
            5,
            sbyte.MaxValue - 1,
            sbyte.MaxValue,
        };

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            ItemArrays itemArrays = (ItemArrays)obj;
            for (int i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], itemArrays._typedPrimitiveArray[i]);
                AssertAreEqual(data[i], ((sbyte[])itemArrays._primitiveArrayInObject)[i]);
                if (NullableSupported())
                {
                    AssertAreEqual(data[i], (sbyte) itemArrays._nullableTypedPrimitiveArray[i]);
                }
            }
            AssertAreEqual(0, itemArrays._typedPrimitiveArray[data.Length]);
            AssertAreEqual(0, ((sbyte[])itemArrays._primitiveArrayInObject)[data.Length]);
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Item item = (Item)values[i];
                AssertAreEqual(data[i], item._typedPrimitive);
                AssertAreEqual(data[i], (sbyte) item._untyped);
                AssertAreEqual(data[i], (sbyte) item._nullablePrimitive);
            }
            Item nullItem = (Item)values[data.Length];
            AssertAreEqual(0, nullItem._typedPrimitive);
            Assert.IsNull(nullItem._untyped);
            Assert.IsNull(nullItem._nullablePrimitive);
		}

        private void AssertAreEqual(sbyte expected, sbyte actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            ItemArrays itemArrays = new ItemArrays();
            itemArrays._typedPrimitiveArray = new sbyte[data.Length + 1];
            System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);

            sbyte[] sbyteArray = new sbyte[data.Length + 1];
            System.Array.Copy(data, 0, sbyteArray, 0, data.Length);
            itemArrays._primitiveArrayInObject = sbyteArray;
            itemArrays._nullableTypedPrimitiveArray = new sbyte?[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                itemArrays._nullableTypedPrimitiveArray[i] = data[i];
            }
			return itemArrays;
        }

        protected override object[] CreateValues()
        {
            Item[] values = new Item[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                Item item = new Item();
                item._typedPrimitive = data[i];
                item._untyped = data[i];
                item._nullablePrimitive = data[i];
				values[i] = item;
            }

            values[data.Length] = new Item();
            return values;
        }

        protected override string TypeName()
        {
            return "sbyte";
        }
    }

}
