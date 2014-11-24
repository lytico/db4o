/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsSystemArrayTestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public System.Array _intArray;

            public System.Array _stringArray;

            public System.Array _nullableIntArray;

            public System.Array _elementArray;
        }

        public class Element
        {
            public string _name;

            public Element(string name)
            {
                _name = name;
            }

            public override bool Equals(object obj)
            {
                Element other = obj as Element;
                if (other == null)
                {
                    return false;
                }
                
                if(_name == null)
                {
                    return other._name == null;
                }
                return _name.Equals(other._name);
            }
        }

        static int[] INT_DATA = new int[] { 0, 1, 2, 3 };

        static string[] STRING_DATA = new string[]{"zero", "one", "two", "three"};

        static int?[] NULLABLE_INT_DATA = new int?[] { 0, 1, 2, 3 };

        static Element[] ELEMENT_DATA = new Element[] { new Element("one") };

        protected override void Store()
        {
            Item item = new Item();
            item._intArray = INT_DATA;
            item._stringArray = STRING_DATA;
            item._nullableIntArray = NULLABLE_INT_DATA;
            item._elementArray = ELEMENT_DATA;
            Store(item);
        }

        public void TestRetrieval()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof (Item));
            int[] boxedIntArray = (int[])item._intArray;
            ArrayAssert.AreEqual(INT_DATA, boxedIntArray);
            string[] boxedStringArray = (string[]) item._stringArray;
            ArrayAssert.AreEqual(STRING_DATA, boxedStringArray);
            int?[] boxedNullableIntArray = (int?[]) item._nullableIntArray;
            ArrayAssert.AreEqual(NULLABLE_INT_DATA, boxedNullableIntArray);
            ArrayAssert.AreEqual((object[])ELEMENT_DATA, (object[])item._elementArray);
        }

        public void TestQuery()
        {
            QueryForInt(1, 1);
            QueryForInt(4, 0);
        }

        private void QueryForInt(int constraint, int expectedCount)
        {
            IQuery q = Db().Query();
            q.Constrain(typeof (Item));
            q.Descend("_intArray").Constrain(constraint);
            Assert.AreEqual(expectedCount, q.Execute().Count);
        }
    }
}
