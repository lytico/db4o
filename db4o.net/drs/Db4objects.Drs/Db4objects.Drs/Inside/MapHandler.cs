/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	public class MapHandler : Db4objects.Drs.Inside.ICollectionHandler
	{
		private readonly IReflectClass _reflectMapClass;

		private readonly ReplicationReflector _reflector;

		public MapHandler(ReplicationReflector reflector)
		{
			_reflector = reflector;
			_reflectMapClass = reflector.ForClass(typeof(IDictionary));
		}

		public virtual bool CanHandleClass(IReflectClass claxx)
		{
			return _reflectMapClass.IsAssignableFrom(claxx);
		}

		public virtual bool CanHandle(object obj)
		{
			return CanHandleClass(_reflector.ForObject(obj));
		}

		public virtual bool CanHandleClass(Type c)
		{
			return CanHandleClass(_reflector.ForClass(c));
		}

		public virtual IEnumerator IteratorFor(object collection)
		{
			IDictionary map = (IDictionary)collection;
			Collection4 result = new Collection4();
			IEnumerator it = map.GetEnumerator();
			while (it.MoveNext())
			{
				DictionaryEntry entry = (DictionaryEntry)it.Current;
				result.Add(entry.Key);
				result.Add(entry.Value);
			}
			return result.GetEnumerator();
		}

		public virtual object EmptyClone(ICollectionSource sourceProvider, object original
			, IReflectClass originalCollectionClass)
		{
			if (sourceProvider.IsProviderSpecific(original) || original is Hashtable)
			{
				return new Hashtable(((IDictionary)original).Count);
			}
			return _reflector.ForObject(original).NewInstance();
		}

		public virtual void CopyState(object original, object destination, ICounterpartFinder
			 counterpartFinder)
		{
			IDictionary originalMap = (IDictionary)original;
			IDictionary destinationMap = (IDictionary)destination;
			destinationMap.Clear();
			IEnumerator it = originalMap.GetEnumerator();
			while (it.MoveNext())
			{
				DictionaryEntry entry = (DictionaryEntry)it.Current;
				object keyClone = counterpartFinder.FindCounterpart(entry.Key);
				object valueClone = counterpartFinder.FindCounterpart(entry.Value);
				destinationMap[keyClone] = valueClone;
			}
		}

		public virtual object CloneWithCounterparts(ICollectionSource sourceProvider, object
			 originalMap, IReflectClass claxx, ICounterpartFinder elementCloner)
		{
			IDictionary original = (IDictionary)originalMap;
			IDictionary result = (IDictionary)EmptyClone(sourceProvider, original, claxx);
			CopyState(original, result, elementCloner);
			return result;
		}
	}
}
