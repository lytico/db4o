/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.TA;
using Db4oTool.Tests.Integration.Model;
using Db4oUnit;

namespace Db4oTool.Tests.Integration
{
	class Application
	{
        const string ItemName = "Item";
		private const string DatabasePath = "pilots.db4o";

		private readonly IDictionary<Type, int> _activations = new Dictionary<Type, int>();

		private void OnActivated(object sender, ObjectInfoEventArgs e)
		{
			Type currentType = e.Object.GetType();
			while (currentType != typeof(object))
			{
				_activations.Add(currentType, ActivationCount(currentType) + 1);
				currentType = currentType.BaseType;
			}
		}

		private int ActivationCount(Type type)
		{
			if (!_activations.ContainsKey(type)) return 0;
			return _activations[type];
		}

		private void Reset()
		{
			_activations.Clear();
		}

		public static int Main()
		{
			new Application().Run();
			return 0;
		}

		private void Run()
		{
			DeleteDatabaseFile();

			try
			{
				WithDatabase(delegate(IObjectContainer db)
				{
					db.Store(new CollectionHolder<Item>("Holder", new Item(ItemName)));
				});

				WithDatabase(delegate(IObjectContainer db)
				{
					CollectionHolder<Item> holder = RetrieveHolder(db);

					AssertCollectionsAreNull(holder);

					AssertItemActivation(
									holder,

									delegate(CollectionHolder<Item> collectionHolder)
									{
										return collectionHolder.List;
									},

									delegate(object obj)
									{
										IList<Item> list = (IList<Item>)obj;
										return list[0];
									},

									typeof(ActivatableList<Item>));
				});

				WithDatabase(delegate(IObjectContainer db)
				{
					CollectionHolder<Item> holder = RetrieveHolder(db);

					AssertCollectionsAreNull(holder);

					AssertItemActivation(
									holder,

									delegate(CollectionHolder<Item> collectionHolder)
									{
										return collectionHolder.Dictionary;
									},

									delegate(object obj)
									{
										IDictionary<string, Item> dictionary = (IDictionary<string, Item>)obj;
										return dictionary[new Item(ItemName).ToString()];
									},

									typeof(ActivatableDictionary<string, Item>));
				});

				TestTransparentPersistence();
			}
			catch (Exception ex)
			{
				Assert.Fail("Test failed.", ex);
			}

			DeleteDatabaseFile();
		}

		private void TestTransparentPersistence()
		{
			WithDatabase(delegate(IObjectContainer db)
			{
				AssertActivationCount(typeof(ActivatableList<Item>), 0);
				CollectionHolder<Item> holder = RetrieveHolder(db);
				holder.List.Clear();
				AssertActivationCount(typeof(ActivatableList<Item>), 1);
			});

			WithDatabase(delegate(IObjectContainer db)
			{
				AssertActivationCount(typeof(ActivatableList<Item>), 0);
				CollectionHolder<Item> holder = RetrieveHolder(db);
				Assert.AreEqual(0, holder.List.Count);
			});
		}

		private static CollectionHolder<Item> RetrieveHolder(IObjectContainer db)
		{
			IObjectSet result = db.Query(typeof(CollectionHolder<Item>));
			CollectionHolder<Item> holder = (CollectionHolder<Item>)result[0];
			return holder;
		}

		private static void AssertCollectionsAreNull(CollectionHolder<Item> holder)
		{
			AssertCollectionIsNull(holder, "_list");
			AssertCollectionIsNull(holder, "_dictionary");
		}

		private void AssertItemActivation(
								CollectionHolder<Item> holder, 
								Func<CollectionHolder<Item>, object> collectionExtractor,
								Func<object, Item> itemExtractor,
								Type collectionType)
		{

			Reset();

			AssertNoActivation(typeof(List<Item>), typeof(Dictionary<string, Item>));

			AssertActivationCount(typeof(Item), 0);
			AssertActivationCount(collectionType, 0);
			Item item = itemExtractor(collectionExtractor(holder));

			AssertActivationCount(collectionType, 1);
			AssertActivationCount(typeof(Item), 0);
			
			Assert.AreEqual(ItemName, item.Name);
			AssertActivationCount(typeof(Item), 1);
		}

		private static void AssertCollectionIsNull(CollectionHolder<Item> holder, string collectionFieldName)
		{
			FieldInfo field = holder.GetType().GetField(collectionFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			Object fieldValue = field.GetValue(holder);
			Assert.IsNull(fieldValue);
		}

		private static void DeleteDatabaseFile()
		{
			File.Delete(DatabasePath);
		}

		private void AssertActivationCount(Type type, int expectedCount)
		{
			Assert.AreEqual(expectedCount, ActivationCount(type), type.Name);
		}

		private void AssertNoActivation(params Type[] collectionTypes)
		{
			foreach (Type t in collectionTypes)
			{
				Assert.AreEqual(0, ActivationCount(t), t.Name);
			}
		}

		private static IEmbeddedConfiguration Config()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.Common.ActivationDepth = 0;
			config.Common.Add(new TransparentPersistenceSupport());
			return config;
		}

		private void WithDatabase(Action<IObjectContainer> block)
		{
			using (IObjectContainer db = Db4oEmbedded.OpenFile(Config(), DatabasePath))
			{
				ListenToActivationEvents(db);
				block(db);
			}
		}
		
		private void ListenToActivationEvents(IObjectContainer db)
		{
			EventRegistryFactory.ForObjectContainer(db).Activated += OnActivated;
			Reset();
		}
	}
}
