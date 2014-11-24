/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	public class CollectionHandlerImpl : Db4objects.Drs.Inside.ICollectionHandler
	{
		private readonly Db4objects.Drs.Inside.ICollectionHandler _mapHandler;

		private readonly ReplicationReflector _reflector;

		public CollectionHandlerImpl(ReplicationReflector reflector)
		{
			_mapHandler = new MapHandler(reflector);
			_reflector = reflector;
		}

		public virtual bool CanHandleClass(IReflectClass claxx)
		{
			if (_mapHandler.CanHandleClass(claxx))
			{
				return true;
			}
			return ReplicationPlatform.IsBuiltinCollectionClass(_reflector, claxx);
		}

		public virtual bool CanHandle(object obj)
		{
			return CanHandleClass(_reflector.ForObject(obj));
		}

		public virtual bool CanHandleClass(Type c)
		{
			return CanHandleClass(_reflector.ForClass(c));
		}

		public virtual object EmptyClone(ICollectionSource sourceProvider, object originalCollection
			, IReflectClass originalCollectionClass)
		{
			if (_mapHandler.CanHandleClass(originalCollectionClass))
			{
				return _mapHandler.EmptyClone(sourceProvider, originalCollection, originalCollectionClass
					);
			}
			ICollection original = (ICollection)originalCollection;
			ICollection clone = ReplicationPlatform.EmptyCollectionClone(sourceProvider, original
				);
			if (null != clone)
			{
				return clone;
			}
			return _reflector.ForClass(original.GetType()).NewInstance();
		}

		public virtual IEnumerator IteratorFor(object collection)
		{
			if (_mapHandler.CanHandleClass(_reflector.ForObject(collection)))
			{
				return _mapHandler.IteratorFor(collection);
			}
			IEnumerable subject = (IEnumerable)collection;
			return Copy(subject).GetEnumerator();
		}

		private Collection4 Copy(IEnumerable subject)
		{
			Collection4 result = new Collection4();
			IEnumerator it = subject.GetEnumerator();
			while (it.MoveNext())
			{
				result.Add(it.Current);
			}
			return result;
		}

		public virtual void CopyState(object original, object destination, ICounterpartFinder
			 counterpartFinder)
		{
			if (_mapHandler.CanHandle(original))
			{
				_mapHandler.CopyState(original, destination, counterpartFinder);
			}
			else
			{
				ReplicationPlatform.CopyCollectionState(original, destination, counterpartFinder);
			}
		}

		public virtual object CloneWithCounterparts(ICollectionSource sourceProvider, object
			 originalCollection, IReflectClass claxx, ICounterpartFinder counterpartFinder)
		{
			if (_mapHandler.CanHandleClass(claxx))
			{
				return _mapHandler.CloneWithCounterparts(sourceProvider, originalCollection, claxx
					, counterpartFinder);
			}
			ICollection original = (ICollection)originalCollection;
			ICollection result = (ICollection)EmptyClone(sourceProvider, originalCollection, 
				claxx);
			CopyState(original, result, counterpartFinder);
			return result;
		}
	}
}
