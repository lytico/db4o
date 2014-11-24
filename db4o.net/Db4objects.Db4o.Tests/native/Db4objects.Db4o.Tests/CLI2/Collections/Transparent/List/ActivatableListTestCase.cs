/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Db4objects.Db4o.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.List
{
	public partial class ActivatableListTestCase : AbstractActivatableCollectionApiTestCase<IList<ICollectionElement>, ICollectionElement>
	{
		#region IList<T> members

		public void TestCorrectContent()
		{
			IteratorAssert.AreEqual(NewPopulatedPlainCollection().GetEnumerator(), SingleCollection().GetEnumerator());
		}

		public void TestCollectionIsNotActivated()
		{
			Assert.IsFalse(Db().IsActive(SingleCollection()));
		}

		public void TestIndexOf()
		{
			const int itemIndex = 2;
			IList<ICollectionElement> collection = SingleCollection();
			int i = collection.Count;
			Assert.AreEqual(itemIndex, collection.IndexOf(NewElement(itemIndex)));
		}

		public void TestIndexerGetter()
		{
			const int indexToTest = 1;
			Assert.AreEqual(NewElement(indexToTest), SingleCollection()[indexToTest]);
		}

		public void TestCopyTo()
		{
			AssertCopy(delegate(ICollectionElement[] elements)
			{
				SingleCollection().CopyTo(elements, 0);
			});
		}

		public void TestIndexerSetter()
		{
			AssertCollectionChange(delegate(IList<ICollectionElement> list)
			{
				const int indexToTest = 1;
				list[indexToTest] = new Element("one-and-half");
			});
		}

		public void TestInsert()
		{
			AssertCollectionChange(delegate(IList<ICollectionElement> list)
			{
				const int insertionIndex = 2;
				const string newItemName = "two-and-half";

				list.Insert(insertionIndex, new Element(newItemName));
			});
		}

		public void TestRemoveAt()
		{
			AssertCollectionChange(delegate(IList<ICollectionElement> list)
			{
				list.RemoveAt(0);
			});
		}

		public void TestRepeatedAdd()
		{
			ICollectionElement four = new Element("four");
			SingleCollection().Add(four);
			Db().Purge();

			ICollectionElement five = new Element("five");
			SingleCollection().Add(five);
			Reopen();

			IList<ICollectionElement> retrieved = SingleCollection();
			Assert.IsTrue(retrieved.Contains(four));
			Assert.IsTrue(retrieved.Contains(five));
		}

		#endregion

		#region List<T> members

		public void TestReadOnly()
		{
			ActivatableList<ICollectionElement> source = SingleActivatableCollection();
			ReadOnlyCollection<ICollectionElement> readOnly = source.AsReadOnly();

			IteratorAssert.AreEqual(NewPopulatedPlainList().GetEnumerator(), readOnly.GetEnumerator());

			source.Add(new Element("n"));
			Assert.IsGreaterOrEqual(0, readOnly.IndexOf(new Element("n")));
		}

		public void TestAddRange()
		{
			SingleActivatableCollection().AddRange(ToBeAdded());
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.AddRange(ToBeAdded());

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleCollection().GetEnumerator());
		}

		public void TestBinarySearch()
		{
			SingleActivatableCollection().Sort();
			Reopen();

			foreach (string name in Names)
			{
				Assert.IsGreaterOrEqual(0, SingleActivatableCollection().BinarySearch(new ActivatableElement(name)));
			}
		}

		public void TestBinarySearch1()
		{
			ActivatableList<ICollectionElement> collection = SingleActivatableCollection();
			collection.Sort();
			int count = collection.Count;
			Reopen();

			foreach (string name in Names)
			{
				Assert.IsGreaterOrEqual(0, SingleActivatableCollection().BinarySearch(0, count, new ActivatableElement(name), SimpleComparer.Instance));
			}
		}

		public void TestBinarySearch2()
		{
			SingleActivatableCollection().Sort();
			Reopen();

			foreach (string name in Names)
			{
				Assert.IsGreaterOrEqual(0, SingleActivatableCollection().BinarySearch(new ActivatableElement(name), SimpleComparer.Instance));
			}
		}

		public void TestCapacity()
		{
			ActivatableList<ICollectionElement> list = SingleActivatableCollection();
			Assert.IsGreater(0, list.Capacity);
			Assert.IsTrue(Db().IsActive(list));

			Reopen();
			list = SingleActivatableCollection();
			list.Capacity = 10;
			Assert.IsTrue(Db().IsActive(list));
		}

		public void TestCopyTo2()
		{
			AssertCopy(delegate(ICollectionElement[] elements)
			{
				SingleActivatableCollection().CopyTo(elements);
			});
		}

		public void TestCopyTo3()
		{
			AssertCopy(delegate(ICollectionElement[] elements)
			{
				SingleActivatableCollection().CopyTo(0, elements, 0, elements.Length);
			});
		}

#if !SILVERLIGHT		
		public void TestExists()
		{
			Assert.IsTrue(SingleActivatableCollection().Exists(delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}

		public void TestFind()
		{
			ICollectionElement found = SingleActivatableCollection().Find(delegate(ICollectionElement candidate) { return candidate.Name == Names[0]; });
			Assert.IsNotNull(found);
		}

		public void TestFindAll()
		{
			Predicate<ICollectionElement> predicate = delegate(ICollectionElement candidate) { return candidate.Name == Names[0]; };

			List<ICollectionElement> expected= NewPopulatedPlainList().FindAll(predicate);
			List<ICollectionElement> actual = SingleActivatableCollection().FindAll(predicate);

			IteratorAssert.SameContent(expected, actual);
		}

		public void TestFindIndexSimplePredicate()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindIndex(delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}

		public void TestFindIndexWithStartIndex()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindIndex(0, delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}

		public void FindIndexWithCount()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindIndex(0, NewPopulatedPlainList().Count, delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}

		public void TestFindLast()
		{
			Predicate<ICollectionElement> match = delegate(ICollectionElement candidate) { return candidate.Name == Names[2]; };

			ICollectionElement expected = NewPopulatedPlainList().FindLast(match);
			ICollectionElement actual = SingleActivatableCollection().FindLast(match);

			Assert.AreEqual(expected, actual);
		}

		public void TestFindLastIndexSimplePredicate()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindLastIndex(delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}
		
		public void TestFindLastIndexWithStartIndex()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindLastIndex(NewPopulatedPlainList().Count - 1, delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}
		
		public void TestFindLastIndexWithCount()
		{
			Assert.IsGreaterOrEqual(0, SingleActivatableCollection().FindLastIndex(NewPopulatedPlainList().Count - 1, NewPopulatedPlainList().Count, delegate(ICollectionElement candidate) { return candidate.Name == Names[1]; }));
		}
#endif

		public void TestForEach()
		{
			int count = 0;
			SingleActivatableCollection().ForEach(delegate(ICollectionElement candidate)
			{
				Assert.IsTrue(candidate.Name.Length > 2);
				count++;
			});

			Assert.IsGreater(0, count);
		}


		public void TestGetRange()
		{
			int startIndex = 1;
			int count = 3;

			List<ICollectionElement> expected = NewPopulatedPlainList().GetRange(startIndex, count);
			List<ICollectionElement> actual = SingleActivatableCollection().GetRange(startIndex, count);

			IteratorAssert.SameContent(expected, actual);
		}

		public void TestIndexOfWithStartIndex()
		{
			ICollectionElement tbf = NewElement(Names.Count - 2);
			const int startIndex = 1;
			
			int expectedIndex = NewPopulatedPlainList().IndexOf(tbf, startIndex);
			int actualIndex = SingleActivatableCollection().IndexOf(tbf, startIndex);

			Assert.AreEqual(expectedIndex, actualIndex);
		}

		public void TestIndexOfWithStartIndexAndCount()
		{
			ICollectionElement tbf = NewElement(Names.Count - 2);
			const int startIndex = 1;
			const int count = 3;

			int expectedIndex = NewPopulatedPlainList().IndexOf(tbf, startIndex, count);
			int actualIndex = SingleActivatableCollection().IndexOf(tbf, startIndex, count);

			Assert.AreEqual(expectedIndex, actualIndex);
		}

		public void TestInsertRange()
		{
			const int index = 2;

			SingleActivatableCollection().InsertRange(index, ToBeAdded());
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.InsertRange(index, ToBeAdded());

			IteratorAssert.SameContent(expected, SingleActivatableCollection());
		}

		public void TestLastIndexOf()
		{
			ICollectionElement tbf = NewElement(1);

			List<ICollectionElement> collection = NewPopulatedPlainList();
			int expected = collection.LastIndexOf(tbf);
			int actual = SingleActivatableCollection().LastIndexOf(tbf);
			
			Assert.AreEqual(expected, actual);
		}

		public void TestLastIndexOfWithStartIndex()
		{
			ICollectionElement tbf = NewElement(1);

			int startIndex = LastIndex();
			int expected = NewPopulatedPlainList().LastIndexOf(tbf, startIndex);
			int actual = SingleActivatableCollection().LastIndexOf(tbf, startIndex);
			
			Assert.AreEqual(expected, actual);
		}

		public void TestLastIndexOfWithStartIndexAndCount()
		{
			ICollectionElement tbf = NewElement(1);

			int startIndex = Names.Count;
			const int count = 5;

			int expected = NewPopulatedPlainList().LastIndexOf(tbf, startIndex, count);
			int actual = SingleActivatableCollection().LastIndexOf(tbf, startIndex, count);

			Assert.AreEqual(expected, actual);
		}

#if !SILVERLIGHT
		public void TestRemoveAll()
		{
			Predicate<ICollectionElement> predicate = delegate(ICollectionElement candidate) { return candidate.Name.Length > 3; };

			int actualCount = SingleActivatableCollection().RemoveAll(predicate);
			Reopen();

			List<ICollectionElement> expectedCollection = NewPopulatedPlainList();
			int expectedCount = expectedCollection.RemoveAll(predicate);

			Assert.AreEqual(expectedCount, actualCount);
			IteratorAssert.SameContent(expectedCollection, SingleActivatableCollection());
		}
#endif

		public void TestRemoveRange()
		{
			const int startIndex = 1;
			const int count = 2;

			SingleActivatableCollection().RemoveRange(startIndex, count);
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.RemoveRange(startIndex, count);

			IteratorAssert.SameContent(expected, SingleActivatableCollection());
		}

		public void TestReverse()
		{
			SingleActivatableCollection().Reverse();
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Reverse();

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleActivatableCollection().GetEnumerator());
		}

		public void TestReverseWithIndexAndCount()
		{
			const int index = 1;
			const int count = 2;

			SingleActivatableCollection().Reverse(index, count);
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Reverse(index, count);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleActivatableCollection().GetEnumerator());
		}

		public void TestSortDefaultComparer()
		{
			ActivatableList<ICollectionElement> actual = SingleActivatableCollection();
			actual.Sort();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Sort();

			IteratorAssert.AreEqual(expected.GetEnumerator(), actual.GetEnumerator());
		}

		public void TestSortWithIndexAndComparer()
		{
			const int index = 1;
			const int count = 3;
			SimpleComparer comparer = new SimpleComparer();

			SingleActivatableCollection().Sort(index, count, comparer);
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Sort(index, count, comparer);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleActivatableCollection().GetEnumerator());
		}
		
		public void TestSortWithComparer()
		{
			SimpleComparer comparer = new SimpleComparer();

			SingleActivatableCollection().Sort(comparer);
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Sort(comparer);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleActivatableCollection().GetEnumerator());
		}

		public void TestSortComparison()
		{
			Comparison<ICollectionElement> comparison = delegate(ICollectionElement lhs, ICollectionElement rhs)
			                                            	{
			                                            		return lhs.CompareTo(rhs);
			                                            	};

			SingleActivatableCollection().Sort(comparison);
			Reopen();

			List<ICollectionElement> expected = NewPopulatedPlainList();
			expected.Sort(comparison);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleActivatableCollection().GetEnumerator());
		}

		public void TestToArray()
		{
			ICollectionElement[] expected = NewPopulatedPlainList().ToArray();
			ICollectionElement[] actual = SingleActivatableCollection().ToArray();

			IteratorAssert.AreEqual(expected, actual.GetEnumerator());
		}

		public void TestTrimExcess()
		{
			ActivatableList<ICollectionElement> collection = SingleActivatableCollection();
			collection.Clear();
			collection.TrimExcess();

			Assert.AreEqual(collection.Count, collection.Capacity);
		}

#if !SILVERLIGHT
		public void TestTrueForAll()
		{
			Assert.IsGreater(0, SingleActivatableCollection().Count);
			Db().Purge();

			int count = 0;
			Assert.IsTrue(SingleActivatableCollection().TrueForAll(delegate(ICollectionElement candidate)
			                                                       	{
			                                                       		count++;  
																		return candidate.Name.Length > 1;
			                                                       	}));

			Assert.IsGreater(0, count);
		}

		public void TestConvertAll()
		{
			Converter<ICollectionElement, string> toString = delegate(ICollectionElement source) { return source.Name; };
			List<string> expectedlNames = NewPopulatedPlainList().ConvertAll(toString);
			List<string> actualNames = SingleActivatableCollection().ConvertAll(toString);

			IteratorAssert.AreEqual(expectedlNames.GetEnumerator(), actualNames.GetEnumerator());
		}
#endif
		#endregion
	}
}
