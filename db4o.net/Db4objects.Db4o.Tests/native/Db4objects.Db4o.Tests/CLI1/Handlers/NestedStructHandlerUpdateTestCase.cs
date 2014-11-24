using System;
using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class NestedStructHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        public struct NestedStruct
        {
            public string name;
            public Guid guid;

            public NestedStruct(string name, Guid guid)
            {
                this.name = name;
                this.guid = guid;
            }
        }

        public class Item
        {
            public NestedStruct _nestedStruct;

            public object _untyped;

			public NestedStruct? _nullableNestedStruct;
        }

        public class ItemArrays
        {
            public NestedStruct[] _nestedStructArray;

            public object[] _untypedArray;

            public object _arrayInObject;

			public NestedStruct?[] _nullableNestedStructArray;
        }

        private static readonly NestedStruct[] data = {
            new NestedStruct("empty", Guid.Empty),
            new NestedStruct("6c673f20-bd63-4b40-9352-6c86b487cf2b", new Guid("6c673f20-bd63-4b40-9352-6c86b487cf2b")),
            new NestedStruct("8dc57ee2-e4cd-423d-b572-3479c3723c27", new Guid("8dc57ee2-e4cd-423d-b572-3479c3723c27")),
            new NestedStruct("5418b227-ac19-48b4-a7ea-b9ab31d0dc82", new Guid("5418b227-ac19-48b4-a7ea-b9ab31d0dc82")),
            new NestedStruct("53d935ff-3042-44ef-9edb-28db194ee43c", new Guid("53d935ff-3042-44ef-9edb-28db194ee43c")),
        };

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            ItemArrays itemArrays = (ItemArrays)obj;
            for (int i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], itemArrays._nestedStructArray[i]);
                AssertAreEqual(data[i], ((NestedStruct[])itemArrays._arrayInObject)[i]);
                if (NullableSupported())
                {
                    AssertAreEqual(data[i], (NestedStruct) itemArrays._nullableNestedStructArray[i]);
                }
                AssertAreEqual(data[i], (NestedStruct) itemArrays._untypedArray[i]);
            }
            if (NullableSupported())
            {
                Assert.IsNull(itemArrays._nullableNestedStructArray[data.Length]);
            }
            Assert.IsNull(itemArrays._untypedArray[data.Length]);
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Item item = (Item)values[i];
                AssertAreEqual(data[i], item._nestedStruct);
                AssertAreEqual(data[i], (NestedStruct) item._untyped);
                AssertAreEqual(data[i], (NestedStruct) item._nullableNestedStruct);
			}
            Item nullItem = (Item)values[data.Length];
            AssertAreEqual(new NestedStruct(null, Guid.Empty), nullItem._nestedStruct);
            Assert.IsNull(nullItem._untyped);
            Assert.IsNull(nullItem._nullableNestedStruct);
		}

        private void AssertAreEqual(NestedStruct expected, NestedStruct actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            ItemArrays itemArrays = new ItemArrays();
            itemArrays._nestedStructArray = new NestedStruct[data.Length];
            System.Array.Copy(data, 0, itemArrays._nestedStructArray, 0, data.Length);

            itemArrays._untypedArray = new object[data.Length + 1];
            System.Array.Copy(data, 0, itemArrays._untypedArray, 0, data.Length);

            NestedStruct[] nestedStructArray = new NestedStruct[data.Length];
            System.Array.Copy(data, 0, nestedStructArray, 0, data.Length);
            itemArrays._arrayInObject = nestedStructArray;
            itemArrays._nullableNestedStructArray = new NestedStruct?[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                itemArrays._nullableNestedStructArray[i] = data[i];
            }
			return itemArrays;
        }

        protected override object[] CreateValues()
        {
            Item[] values = new Item[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                Item item = new Item();
                item._nestedStruct = data[i];
                item._untyped = data[i];
                item._nullableNestedStruct = data[i];
				values[i] = item;
            }
            values[data.Length] = new Item();
            return values;
        }

        protected override string TypeName()
        {
            return "nestedstruct";
        }

        protected override bool DefragmentInReadWriteMode()
        {
            return true;
        }

    }
}
