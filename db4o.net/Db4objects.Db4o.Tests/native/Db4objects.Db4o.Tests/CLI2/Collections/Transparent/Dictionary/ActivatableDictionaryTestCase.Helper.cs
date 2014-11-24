/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;

#if !CF && !SILVERLIGHT
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Db4oUnit;
#endif

using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.Dictionary
{
	partial class ActivatableDictionaryTestCase
	{
		protected override IDictionary<string, ICollectionElement> NewPlainCollection()
		{
			return new Dictionary<string, ICollectionElement>();
		}

		protected override IDictionary<string, ICollectionElement> SingleCollection()
		{
			var holder = (CollectionHolder<IDictionary<string, ICollectionElement>>)RetrieveOnlyInstance(typeof(CollectionHolder<IDictionary<string, ICollectionElement>>));
			return holder.Collection;
		}

		protected override IDictionary<string, ICollectionElement> NewActivatableCollection(IDictionary<string, ICollectionElement> template)
		{
			return new ActivatableDictionary<string, ICollectionElement>(template);
		}

		protected override IDictionary<string, ICollectionElement> NewActivatableCollection()
		{
			return new ActivatableDictionary<string, ICollectionElement>();
		}
		
		protected override KeyValuePair<string, ICollectionElement> NewElement(string value)
		{
			return new KeyValuePair<string, ICollectionElement>(value, new Element(value));
		}

		protected override KeyValuePair<string, ICollectionElement> NewActivatableElement(string value)
		{
			return new KeyValuePair<string, ICollectionElement>("activatable-" + value, new ActivatableElement(value));
		}

		private ICollectionElement NewItem(string key)
		{
			return NewElement(key).Value;
		}

#if !CF && !SILVERLIGHT
		private void AssertSerializable(IDictionary<string, ICollectionElement> dictionary)
		{
			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, dictionary);
			stream.Position = 0;
			formatter = new BinaryFormatter();
			IDictionary<string, ICollectionElement> actual = (IDictionary<string, ICollectionElement>)formatter.Deserialize(stream);
			Assert.AreEqual(NewPopulatedPlainCollection()[ExistingKey], actual[ExistingKey]);
			Assert.IsFalse(actual.ContainsKey(NonExistingKey));
		}
#endif
	}
}
