using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class StructHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        public struct Foo
        {
            public string name;

            public int value;

            public Foo(string name, int value)
            {
                this.name = name;

                this.value = value;
            }

            public override bool Equals(object obj)
            {
                if ((object)this == obj) return true;
                
                if (!(obj is Foo)) return false;

                Foo other = (Foo)obj;

                return (value == other.value) && (name == null ? other.name == null : other.name == name);
            }
        }

        public class Item
        {
            public Foo _foo;

            public object _untyped;

			public Foo? _nullableFoo;
        }

        public class ItemArrays
        {
            public Foo[] _fooArray;

            public object[] _untypedArray;

            public object _aArrayInObject;

			public Foo?[] _nullableFooArray;
        }

        private static readonly Foo[] data = new Foo[] {
            new Foo("MinValue", int.MinValue),
            new Foo("MinValue + 1", int.MinValue + 1),
            new Foo("Minus Five", -5),
            new Foo("Minus One", -1),
            new Foo("Zero", 0),
            new Foo("One", 1),
            new Foo("Five", 5),
            new Foo("MaxValue - 1", int.MaxValue - 1),
            new Foo("MaxValue", int.MaxValue),
        };

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            ItemArrays itemArrays = (ItemArrays)obj;
            Foo[] fooArray = (Foo[])itemArrays._aArrayInObject;
            for (int i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], itemArrays._fooArray[i]);
                AssertAreEqual(data[i], (Foo)itemArrays._untypedArray[i]);
                AssertAreEqual(data[i], fooArray[i]);
                if (NullableSupported())
                {
                    AssertAreEqual(data[i], (Foo) itemArrays._nullableFooArray[i]);
                }
            }
            Assert.IsNull(itemArrays._untypedArray[data.Length]);
            if (NullableSupported())
            {
                Assert.IsNull(itemArrays._nullableFooArray[data.Length]);
            }
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Item item = (Item)values[i];
                AssertAreEqual(data[i], item._foo);
                AssertAreEqual(data[i], (Foo) item._untyped);
                AssertAreEqual(data[i], (Foo) item._nullableFoo);
			}

            Item nullItem = (Item)values[values.Length - 1];

            AssertAreEqual(new Foo(null, 0), nullItem._foo);
            Assert.IsNull(nullItem._untyped);
            Assert.IsNull(nullItem._nullableFoo);
		}

        private void AssertAreEqual(Foo expected, Foo actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            ItemArrays itemArrays = new ItemArrays();
            itemArrays._fooArray = new Foo[data.Length];
            System.Array.Copy(data, 0, itemArrays._fooArray, 0, data.Length);

            itemArrays._untypedArray = new object[data.Length + 1];
            System.Array.Copy(data, 0, itemArrays._untypedArray, 0, data.Length);

            Foo[] fooArray = new Foo[data.Length];
            System.Array.Copy(data, 0, fooArray, 0, data.Length);

            itemArrays._aArrayInObject = fooArray;
            itemArrays._nullableFooArray = new Foo?[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                itemArrays._nullableFooArray[i] = data[i];
            }
			return itemArrays;
        }

        protected override object[] CreateValues()
        {
            Item[] values = new Item[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                Item item = new Item();
                item._foo = data[i];
                item._untyped = data[i];
                item._nullableFoo = data[i];
				values[i] = item;
            }
            values[data.Length] = new Item();
            return values;
        }

        protected override string TypeName()
        {
            return "struct";
        }

        protected override bool DefragmentInReadWriteMode()
        {
            return true;
        }

    }
}
