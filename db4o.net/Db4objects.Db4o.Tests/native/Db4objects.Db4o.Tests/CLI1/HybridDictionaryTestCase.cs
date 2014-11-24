#if !SILVERLIGHT

using System;
using System.Collections.Specialized;
using Db4objects.Db4o.Internal;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class HybridDictionaryTestCase : FixtureBasedTestSuite
	{
		public override Type[] TestUnits()
		{
			return new Type[] { typeof(TestUnit) };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			{
				new SubjectFixtureProvider(new object[] {
					new ItemCounts(5, 10),
					new ItemCounts(10, 5)
				})
			};
		}

		class ItemCounts
		{
			public readonly int Initial;
			public readonly int Update;

			public ItemCounts(int initialCount, int updateCount)
			{
				Initial = initialCount;
				Update = updateCount;
			}
		}

		public class TestUnit : AbstractDb4oTestCase
		{
			protected override void Store()
			{
				Holder holder = new Holder();
				AddItemsTo(holder, InitialItemCount());
				Store(holder);
			}

			protected override void Configure(Db4objects.Db4o.Config.IConfiguration config)
			{
				config.ObjectClass(typeof(Holder)).CascadeOnUpdate(true);
			}

			public void Test()
			{
				AssertHolder(InitialItemCount());
			}

			public void TestUpdate()
			{
				UpdateHolder();
				Reopen();
				AssertHolder(UpdateItemCount());
			}

			private void UpdateHolder()
			{
				Holder holder = RetrieveOnlyInstance<Holder>();
				holder.Clear();
				AddItemsTo(holder, UpdateItemCount());
				Store(holder);
			}

			private void AssertHolder(int expectedItemCount)
			{
				Holder holder = RetrieveOnlyInstance<Holder>();
				Assert.AreEqual(expectedItemCount, holder.Count);
				for (int i = 0; i < expectedItemCount; ++i)
				{
					Assert.AreEqual(i, holder[new Item(i)]);
				}
			}

			private void AddItemsTo(Holder holder, int count)
			{
				for (int i = 0; i < count; ++i)
				{
					holder.Add(new Item(i));
				}
			}

			private int InitialItemCount()
			{
				return ItemCounts().Initial;
			}

			private int UpdateItemCount()
			{
				return ItemCounts().Update;
			}

			private ItemCounts ItemCounts()
			{
				return ((ItemCounts)SubjectFixtureProvider.Value());
			}
		}

		public class Holder
		{
			private HybridDictionary _dictionary;

			public Holder()
			{
				_dictionary = new HybridDictionary();
			}

			public int Count
			{
				get { return _dictionary.Count; }
			}

			public int this[Item item]
			{
				get { return (int) _dictionary[item];  }
			}

			public void Add(Item item)
			{
				_dictionary.Add(item, item.id);
			}

			public void Clear()
			{
				_dictionary.Clear();
			}
		}

		public class Item
		{
			public int id;

			public Item(int id)
			{
				this.id = id;
			}

			public override int GetHashCode()
			{
				return id;
			}

			public override bool Equals(object obj)
			{
				Item other = obj as Item;
				if (other == null)
				{
					return false;
				}

				return id == other.id;
			}
		}
	}
}
#endif