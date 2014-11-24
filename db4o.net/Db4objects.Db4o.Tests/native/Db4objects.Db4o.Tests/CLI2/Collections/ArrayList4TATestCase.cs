using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.TA;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    class ArrayList4TATestCase : TransparentActivationTestCaseBase, IOptOutSilverlight
    {
#if !SILVERLIGHT
        private const int SIZE = 50;

        protected override void Store()
        {
            IList<string> list = new ArrayList4<string>();
            for(int i=0; i < SIZE; i++)
            {
            	list.Add(i.ToString());
            }
            Store(list);
        }

        private static object GetField(object obj, string fieldName)
        {
        	return Reflection4.GetFieldValue(obj, fieldName);
        }

    	private ArrayList4<T> RetrieveOnlyInstance<T>()
        {
            ArrayList4<T> list = (ArrayList4<T>)RetrieveOnlyInstance(typeof(ArrayList4<T>));
            AssertRetrievedItem(list);
            return list;
        }

        private void AssertRetrievedItem<T>(IList<T> list)
        {
#if CF
            object[] elements = (object[]) GetField(list, "elements");
            Assert.AreEqual(10, elements.Length);
            foreach (object obj in elements)
            {
                Assert.IsNull(obj);
            }
#else 
// TODO COR-1261
//            Assert.IsNull(GetField(list, "elements"));
#endif
			Assert.AreEqual(0, GetField(list, "listSize"));
        }

        public void TestIndexOf()
        {
            ArrayList4Asserter.AssertIndexOf(
                                    RetrieveOnlyInstance<string>(),
                                    new IndexOfItems<string, int>( "10", 10),
                                    new IndexOfItems<string, int>("911", -1),
                                    new IndexOfItems<string, int>("0", 0),
                                    new IndexOfItems<string, int>("49", 49),
                                    new IndexOfItems<string, int>("50", -1));
        }

        public void TestAddDifferentTypes()
        {
            ArrayList4Asserter.TestAddDifferentTypes(RetrieveOnlyInstance<string>(), 20);
        }

        public void TestLowerBound()
        {
            ArrayList4Asserter.AssertLowerBound(RetrieveOnlyInstance<string>());
        }

        public void TestUpperBound()
        {
            ArrayList4Asserter.AssertUpperBound();
        }

        public void TestContains()
        {
            ArrayList4Asserter.AssertContains(
                                    RetrieveOnlyInstance<string>(),
                                    "there's no such thing as free lunch!",
                                    "0.1",
                                    " 10");
        }

        public void TestCopyToWithNullTarget()
        {
            ArrayList4Asserter.AssertCopyToWithNullTarget(RetrieveOnlyInstance<string>());
        }

        public void TestCopyToMultiDimensionalArray()
        {
            ArrayList4Asserter.AssertCopyToWithMultiDimensionalArray(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestCopyToInvalidIndex()
        {
            ArrayList4Asserter.AssertCopyToInvalidIndex(RetrieveOnlyInstance<string>());
        }

        public void TestRemove()
        {
            ArrayList4Asserter.AssertRemove(
                                RetrieveOnlyInstance<string>(),
                                new IndexOfItems<string, bool>("-1", false),
                                new IndexOfItems<string, bool>(SIZE.ToString(), false),
                                new IndexOfItems<string, bool>((SIZE - 1).ToString(), true),
                                new IndexOfItems<string, bool>((SIZE - 1).ToString(), false),
                                new IndexOfItems<string, bool>("20", true),
                                new IndexOfItems<string, bool>("0", true),
                                new IndexOfItems<string, bool>("1", true));
        }

        public void TestRemoveAt()
        {
            IList<string> list = RetrieveOnlyInstance<string>();
            ArrayList4Asserter.AssertRemoveAt(
                    list,
                    new IndexOfItems<int, Type>(-1, typeof(ArgumentOutOfRangeException)),
                    new IndexOfItems<int, Type>(list.Count, typeof(ArgumentOutOfRangeException)),
                    new IndexOfItems<int, Type>(0, typeof(int)),
                    new IndexOfItems<int, Type>(list.Count-2, typeof(int)));
        }

        public void TestEnumerator()
        {
            ArrayList4Asserter.AssertEnumerable(
                                    RetrieveOnlyInstance<string>(),
                                    GetEnumerable(SIZE));
        }

        public void TestIndexer()
        {
            ArrayList4<string> list = RetrieveOnlyInstance<string>();
            AssertRetrievedItem(list);
            Assert.AreEqual("10", list[10]);
        }

        private static IEnumerable<string> GetEnumerable(int size)
        {
            for(int i = 0 ; i < size; i++)
            {
                yield return i.ToString();
            }
        }
#endif
    }
}
