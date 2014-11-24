using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class UIntHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        public class Item
        {
            public uint _typedPrimitive;

            public object _untyped;

			public uint? _nullablePrimitive;
        }

        public class ItemArrays
        {
            public uint[] _typedPrimitiveArray;

            public object _primitiveArrayInObject;

			public uint?[] _nullableTypedPrimitiveArray;
        }

        private static readonly uint[] data = new uint[] {
            uint.MinValue,
            uint.MinValue + 1,
            5,
            uint.MaxValue - 1,
            uint.MaxValue,
        };

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            ItemArrays itemArrays = (ItemArrays)obj;
            for (int i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], itemArrays._typedPrimitiveArray[i]);
                AssertAreEqual(data[i], ((uint[])itemArrays._primitiveArrayInObject)[i]);
                if (NullableSupported())
                {
                    AssertAreEqual(data[i], (uint) itemArrays._nullableTypedPrimitiveArray[i]);
                }
            }
            AssertAreEqual(0, itemArrays._typedPrimitiveArray[data.Length]);
            AssertAreEqual(0, ((uint[])itemArrays._primitiveArrayInObject)[data.Length]);
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Item item = (Item)values[i];
                AssertAreEqual(data[i], item._typedPrimitive);
                AssertAreEqual(data[i], (uint) item._untyped);
                AssertAreEqual(data[i], (uint) item._nullablePrimitive);
			}
            Item nullItem = (Item)values[data.Length];
            AssertAreEqual(0, nullItem._typedPrimitive);
            Assert.IsNull(nullItem._untyped);
            Assert.IsNull(nullItem._nullablePrimitive);
		}

        private void AssertAreEqual(uint expected, uint actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            ItemArrays itemArrays = new ItemArrays();
            itemArrays._typedPrimitiveArray = new uint[data.Length + 1];
            System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);

            uint[] uintArray = new uint[data.Length + 1];
            System.Array.Copy(data, 0, uintArray, 0, data.Length);
            itemArrays._primitiveArrayInObject = uintArray;
            itemArrays._nullableTypedPrimitiveArray = new uint?[data.Length + 1];
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
            return "uint";
        }
    }

}
