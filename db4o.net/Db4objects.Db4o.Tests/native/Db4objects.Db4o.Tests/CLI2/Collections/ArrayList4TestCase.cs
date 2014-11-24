#if !SILVERLIGHT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Db4objects.Db4o.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    internal class ArrayList4TestCase : ITestLifeCycle
    {
        #region ITestLifeCycle Members

        public void SetUp()
        {
        }

        public void TearDown()
        {
        }

        #endregion

        #region Test Methods 

		public void TestInitialCapacityDoesntImplyCount()
		{
			ArrayList4<int> list = new ArrayList4<int>(10);
			Assert.AreEqual(0, list.Count);
			Assert.Expect(typeof(ArgumentOutOfRangeException), delegate { list[0] = 42; });
		}

        public void TestAddDifferentTypes()
        {
            ArrayList4Asserter.TestAddDifferentTypes(ArrayList4Asserter.CreateArrayListAndAssertValues(10), "No way my friend");
        }

        public void TestLowerBound()
        {
            ArrayList4Asserter.AssertLowerBound(ArrayList4Asserter.CreateArrayListAndAssertValues(10));
        }

        public void TestUpperBound()
        {
            ArrayList4Asserter.AssertUpperBound();
        }

        public void TestItems()
        {
            ArrayList4Asserter.AssertItems();
        }

        public void TestAddItems()
        {
            ArrayList4Asserter.AssertAddItem();
        }

        public void TestIsReadOnly()
        {
            ArrayList4Asserter.AssertIsReadOnly();
        }

        public void TestClear()
        {
            ArrayList4Asserter.AssertClear();
        }

        public void TestContains()
        {
            ArrayList4Asserter.AssertContains(
                            ArrayList4Asserter.CreateArrayList(10),
                            -1,
                            ArrayList4Asserter.ValueForIndex(10) + 1);
        }

        public void TestCopyTo()
        {
            ArrayList4Asserter.AssertCopyTo(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestCopyToWithInvalidSize()
        {
            ArrayList4Asserter.AssertCopyToWithInvalidSize(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestCopyToWithNullTarget()
        {
            ArrayList4Asserter.AssertCopyToWithNullTarget(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestCopyToMultiDimensionalArray()
        {
            ArrayList4Asserter.AssertCopyToWithMultiDimensionalArray(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestCopyToInvalidIndex()
        {
            ArrayList4Asserter.AssertCopyToInvalidIndex(ArrayList4Asserter.CreateArrayList(10));
        }

        public void TestRemove()
        {
            ArrayList4Asserter.AssertRemove<int>(ArrayList4Asserter.CreateArrayListAndAssertValues(10));
        }

        public void TestRemoveAt()
        {
            ArrayList4Asserter.AssertRemoveAt(
                            ArrayList4Asserter.CreateArrayListAndAssertValues(10),
                            new IndexOfItems<int, Type>(-1, typeof(ArgumentOutOfRangeException)),
                            new IndexOfItems<int, Type>(10, typeof(ArgumentOutOfRangeException)),
                            new IndexOfItems<int, Type>(50, typeof(ArgumentOutOfRangeException)),
                            new IndexOfItems<int, Type>(5, typeof(int)));
        }

		public void TestIndexOfOnEmptyList()
		{
			ArrayList4<int> list = new ArrayList4<int>();
			Assert.AreEqual(-1, list.IndexOf(0));
		}

    	public void TestIndexOf()
        {
            ArrayList4Asserter.AssertIndexOf(
                ArrayList4Asserter.CreateArrayListAndAssertValues(10),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(0), 0),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(10), -1),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(-8), -1),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(5), 5));
            
        }

        public void TestInsert()
        {
            ArrayList4Asserter.AssertInsert(
                            new ArrayList4<int>(0),
                            new int[] {1, 2, 3},
                            new IndexOfItems<int, int>(1,0),
                            new IndexOfItems<int, int>(2,1),
                            new IndexOfItems<int, int>(3,2));

            ArrayList4Asserter.AssertInsert(
                            new ArrayList4<int>(new int[] {3, 2, 1}),
                            new int[] { 3, 4, 2, 5, 1, 6 },
                            new IndexOfItems<int, int>(4, 1),
                            new IndexOfItems<int, int>(5, 3),
                            new IndexOfItems<int, int>(6, 5));
        }

        public void TestEnumerator()
        {
            ArrayList4Asserter.AssertEnumerable(
                        ArrayList4Asserter.CreateArrayListAndAssertValues(10),
                        GetEnumerable(0, 10));
        }

        public void TestFailEnumerator()
        {
            ArrayList4Asserter.AssertFailEnumerator<int>(
                        ArrayList4Asserter.CreateArrayListAndAssertValues(10),
                        GetEnumerable(0, 10),
                        6,
                        20);
        }

        public void TestToString()
        {
            ArrayList4Asserter.AssertToString(
                                    ArrayList4Asserter.CreateArrayListAndAssertValues(10), "ArrayList4<Int32> (Count=10)");
            
            ArrayList4Asserter.AssertToString(
                                    new ArrayList4<int>(0), "ArrayList4<Int32> (Count=0)");
            
            ArrayList4Asserter.AssertToString(
                                    new ArrayList4<string>(0), "ArrayList4<String> (Count=0)");
        }

        public void TestSort()
        {
            ArrayList4<int> list = (ArrayList4<int>)ArrayList4Asserter.CreateArrayListAndAssertValues(100);
            list.Sort(0, list.Count, new InverseComparer());

            Assert.IsGreaterOrEqual(1, list.Count);
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i-1] < list[i])
                {
                    Assert.Fail(String.Format("Indexes ({0}, {1}). Values ({2}, {3})", i, i - 1, list[i], list[i - 1]));
                }
            }
        }

        public void TestBinarySearch()
        {
            ArrayList4Asserter.AssertBinarySearch(
                (ArrayList4<int>) ArrayList4Asserter.CreateArrayListAndAssertValues(100),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(0), 0),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(99), 99),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(50), 50),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(100), -1),
                new IndexOfItems<int, int>(ArrayList4Asserter.ValueForIndex(-1), -1));
        }

        internal class InverseComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y - x;
            }
        }

        private static IEnumerable<int> GetEnumerable(int start, int end)
        {
            for(int i = start; i < end; i++)
            {
                yield return ArrayList4Asserter.ValueForIndex(i);
            }
        }

        #endregion
    }
}
#endif