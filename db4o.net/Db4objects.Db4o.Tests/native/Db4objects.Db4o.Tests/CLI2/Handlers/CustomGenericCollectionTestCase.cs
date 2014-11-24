using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class CollectionDerivedSecondGenericArg<T,X> : List<X>
	{
	}

	public class Base
	{
	}

	public class ICollectionImplSecondGenericArg<T,X> : Base, ICollection<X>
	{
#if SILVERLIGHT
		public List<X> coll = new List<X>();
#else
		private List<X> coll = new List<X>();
#endif

		public IEnumerator<X> GetEnumerator()
		{
			return coll.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(X item)
		{
			coll.Add(item);
		}

		public void Clear()
		{
			coll.Clear();
		}

		public bool Contains(X item)
		{
			return coll.Contains(item);
		}

		public void CopyTo(X[] array, int arrayIndex)
		{
			coll.CopyTo(array, arrayIndex);
		}

		public bool Remove(X item)
		{
			return coll.Remove(item);
		}

		public int Count
		{
			get { return coll.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public X this[int i]
		{
			get { return coll[i]; }
		}
	}

	public class OpenBaseType<T> : ICollectionImplSecondGenericArg<T, string>
	{
	}

	public class DerivedFromOpenType : OpenBaseType<int>
	{
	}

	public class DerivedFromListNoGenericArgs : List<string>
	{
	}

	public class Item
	{
		public DerivedFromOpenType openTypeDerived = new DerivedFromOpenType();

		public DerivedFromListNoGenericArgs dflnga = new DerivedFromListNoGenericArgs();

		public CollectionDerivedSecondGenericArg<int, string> coll = new CollectionDerivedSecondGenericArg<int, string>();

		public Item(params string[] args)
		{
			if (args == null)
			{
				return;
			}

			foreach (var s in args)
			{
				coll.Add(s);
				openTypeDerived.Add(s);
				dflnga.Add(s);
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as Item;
			if (other == null) return false;

			if ( (other.coll != coll) && (other.coll == null || coll == null)) return false;

			if (other.coll.Count != coll.Count) return false;
			if (other.openTypeDerived.Count != openTypeDerived.Count) return false;

			for (int i = 0; i < coll.Count; i++)
			{
				if (other.coll[i] != coll[i]) return false;
				if (other.openTypeDerived[i] != openTypeDerived[i]) return false;
				if (other.coll[i] != coll[i]) return false;
			}

			return true;
		}
	}

	public class CustomGenericCollectionTestCase : AbstractDb4oTestCase
	{
#if !CF_3_5
		protected override void Store()
		{
			Store(NewItem());
		}

		public void Test()
		{
			var retrieved = RetrieveOnlyInstance<Item>();
			Assert.AreEqual(NewItem(), retrieved);
		}
		
		private static Item NewItem()
		{
			return new Item("foo", "bar", "baz");
		}
#endif
	}
}
