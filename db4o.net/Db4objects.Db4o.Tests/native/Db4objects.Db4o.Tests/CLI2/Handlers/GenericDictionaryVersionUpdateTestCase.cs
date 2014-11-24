/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Collections;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
    class GenericDictionaryVersionUpdateTestCase : HandlerUpdateTestCaseBase
    {
        class Item<K,V>
        {
            public IDictionary<K,V> dictionary;
            public object untypedGenericDictionary;
            public IDictionary<SimpleItem, SimpleItem> simpleItemDictionary;

            public Item(IDictionary<K, V> list_, object untypedGenericDictionary_, IDictionary<SimpleItem, SimpleItem> simpleItemDictionary_)
            {
                dictionary = list_;
                untypedGenericDictionary = untypedGenericDictionary_;
                simpleItemDictionary = simpleItemDictionary_;
            }
        }

        class ItemArray
        {
            public IDictionary<int,int>[] arrayOfIntDictionary;
            public IDictionary<SimpleItem, SimpleItem>[] arrayOfSimpleItemDictionary;
            public object genericDictionaryArrayInObject;
            public object[] genericDictionaryArrayInObjectArray;

            public ItemArray(
                        IDictionary<int,int>[] arrayOfIntDictionary_,
                        IDictionary<SimpleItem, SimpleItem>[] arrayOfSimpleItemDictionary_,
                        object genericDictionaryArrayInObject_,
                        object[] genericDictionaryArrayInObjectArray_)
            {
                arrayOfIntDictionary = arrayOfIntDictionary_;
                arrayOfSimpleItemDictionary = arrayOfSimpleItemDictionary_;
                genericDictionaryArrayInObject = genericDictionaryArrayInObject_;
                genericDictionaryArrayInObjectArray = genericDictionaryArrayInObjectArray_;
            }
        }

        sealed class SimpleItem
        {
            public int foo;

            public SimpleItem(int foo_)
            {
                foo = foo_;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (obj.GetType() != this.GetType()) return false;

                SimpleItem item = (SimpleItem)obj;
                return item.foo == foo;
            }
        }

        private static IDictionary<int,int> intDictionary1()
        {
            return NewDictionary<int>(new int[] {Int32.MinValue, 0, Int32.MaxValue});
        }

        private static IDictionary<int, int> intDictionary2()
        {
            return NewDictionary<int>(new int[] { 1, 2, 3 });
        }

        private static IDictionary<K, K> NewDictionary<K>(Array arr)
        {
            IDictionary <K,K> dict = new Dictionary<K, K>();
            foreach(K obj in arr)
            {
                dict[obj] = obj;
            }
            return dict;
        }

        private static IDictionary<int?, int?>  nullableIntDictionary1()
        {
            return NewDictionary<int?>(new int?[] { 1, 2, 3 });
        }

        private static IDictionary<SimpleItem, SimpleItem> simpleItemDictionary1()
        {
            return NewDictionary<SimpleItem>(new SimpleItem[] { new SimpleItem(100), new SimpleItem(200) });
        }

        private static IDictionary<SimpleItem, SimpleItem> simpleItemDictionary2()
        {
            return NewDictionary<SimpleItem>(new SimpleItem[] { new SimpleItem(-1), new SimpleItem(42) });
        }

        private static IDictionary<SimpleItem, SimpleItem> simpleItemEmptyDictionary()
        {
            return new Dictionary<SimpleItem, SimpleItem>();
        }

        private static IDictionary<string, string> stringDictionary1()
        {
            return NewDictionary<string>(new string[] { "Adriano", "Norberto", String.Empty });
        }

        private static IDictionary<string, string> stringDictionary2()
        {
            return NewDictionary<string>(new string[] { "Foo", "Bar", String.Empty });
        }

        protected override string TypeName()
        {
            return "Generic List Version Update";
        }

        protected override object[] CreateValues()
        {
            return new object[] {
                                    new Item<int, int>( intDictionary1(), intDictionary2(), null),
                                    new Item<string, string>(stringDictionary1(), stringDictionary2(), simpleItemDictionary1()),

                                    // TODO: Dictionaries of nullable types are broken after retrieval
                                    //       The issue becomes apparent when the new Typehandler kicks in
                                    //       and tries to store them.
                                    // new Item<int?, int?>( nullableIntDictionary1(), stringDictionary1(), simpleItemDictionary1()),
                        };
        }

        protected override object CreateArrays()
        {
            IDictionary<int,int>[] intDictionary = new IDictionary<int, int>[] { intDictionary1(), intDictionary2() };

            IDictionary<SimpleItem, SimpleItem>[] simpleItemDictionary = new IDictionary<SimpleItem, SimpleItem>[]
                                                 {
                                                     simpleItemDictionary1(), 
                                                     simpleItemDictionary2(),
                                                     simpleItemEmptyDictionary(),
                                                 };

            return new ItemArray(intDictionary, simpleItemDictionary, simpleItemDictionary, intDictionary);
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            AssertItem(objectContainer, (Item<int, int>)values[0], intDictionary1(), intDictionary2(), null);
            AssertItem(objectContainer, (Item<string, string>)values[1], stringDictionary1(), stringDictionary2(), simpleItemDictionary1());

            //TODO: Enable after fixing nullable array handling.
            // AssertItem(objectContainer, (Item<int?, int?>)values[2], nullableIntDictionary1(), stringDictionary1(), simpleItemDictionary1());
        }

        private void AssertItem<T, R>(IExtObjectContainer objectContainer, Item<T,T> tba, IDictionary<T,T> dictionary, IDictionary<R,R> untypedGenericList, IDictionary<SimpleItem, SimpleItem> simpleItemDictionary)
        {
            Assert.IsNotNull(tba);
            AssertDictionary(dictionary, tba.dictionary);
            AssertQuery(objectContainer, tba, tba.dictionary, "dictionary");
            AssertDictionary(untypedGenericList, tba.untypedGenericDictionary as IDictionary<R, R>);
            AssertDictionary(simpleItemDictionary, tba.simpleItemDictionary);
        }

        private void AssertQuery<T>(IExtObjectContainer objectContainer, Item<T,T> item, IDictionary<T,T> dictionary, string fieldName)
        {
            if (Db4oHandlerVersion() < 4)
            {
                return;
            }
            ICollection<T> keys = dictionary.Keys;
            if (keys.Count < 1)
            {
                return;
            }
            IQuery query = objectContainer.Query();
            query.Constrain(typeof(Item<T,T>));
            IEnumerator<T> enumerator = keys.GetEnumerator();
            enumerator.MoveNext();
            object constraint = enumerator.Current;
            query.Descend(fieldName).Constrain(constraint);
            IObjectSet objectSet = query.Execute();
            Assert.AreEqual(1, objectSet.Count);
            Item<T,T> queriedItem = (Item<T,T>)objectSet.Next();
            Assert.AreSame(item, queriedItem);
        }

        private void AssertDictionary<T, S>(IDictionary<T,T> expected, IDictionary<S,S> actual)
        {
            if (expected != null)
            {
                Assert.IsNotNull(actual);
                Iterator4Assert.AreEqual(expected.GetEnumerator(), actual.GetEnumerator());
                foreach (T key in expected.Keys)
                {
                    Assert.AreEqual(key, expected[key]);
                }
            }
            else
            {
                Assert.IsNull(actual);
            }
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            ItemArray itemArray = obj as ItemArray;
            Assert.IsNotNull(itemArray);

            AssertDictionary(
                        new IDictionary<int, int>[] { intDictionary1(), intDictionary2() },
                        itemArray.arrayOfIntDictionary);

            AssertDictionary(
                    new IDictionary<SimpleItem, SimpleItem>[] { simpleItemDictionary1(), simpleItemDictionary2(), simpleItemEmptyDictionary() },
                    itemArray.arrayOfSimpleItemDictionary);

            AssertDictionary(
                        new IDictionary<SimpleItem, SimpleItem>[] { simpleItemDictionary1(), simpleItemDictionary2(), simpleItemEmptyDictionary() },
                        (IDictionary<SimpleItem, SimpleItem>[])itemArray.genericDictionaryArrayInObject);

            AssertDictionary(
                        new IDictionary<int, int>[] { intDictionary1(), intDictionary2() },
                        (IDictionary<int, int>[])itemArray.genericDictionaryArrayInObjectArray);
        }

        private void AssertDictionary<T>(IDictionary<T,T>[] expected, IDictionary<T,T>[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                AssertDictionary(expected[i], actual[i]);
            }
        }

        protected override void ConfigureForTest(IConfiguration config)
        {
            if(Db4oMajorVersion() > 7)
            {
                return;
            }
            if(Db4oMajorVersion() == 7)
            {
                if(Db4oMinorVersion() > 5)
                {
                    return;
                }
            }
            config.ExceptionsOnNotStorable(false);
        }

    }
}
