/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Internal.Reflect.Generic
{
	public class KnownClassesCollector
	{
		private readonly ObjectContainerBase _container;

		private readonly KnownClassesRepository _repository;

		public KnownClassesCollector(ObjectContainerBase container, KnownClassesRepository
			 repository)
		{
			_container = container;
			_repository = repository;
		}

		public virtual IReflectClass[] Collect()
		{
			Collection4 classes = new Collection4();
			CollectKnownClasses(classes);
			return (IReflectClass[])classes.ToArray(new IReflectClass[classes.Size()]);
		}

		private void CollectKnownClasses(Collection4 classes)
		{
			IListener4 collectingListener = NewCollectingClassListener(classes);
			_repository.AddListener(collectingListener);
			try
			{
				CollectKnownClasses(classes, Iterators.Copy(_repository.Classes()));
			}
			finally
			{
				_repository.RemoveListener(collectingListener);
			}
		}

		private IListener4 NewCollectingClassListener(Collection4 classes)
		{
			return new _IListener4_37(this, classes);
		}

		private sealed class _IListener4_37 : IListener4
		{
			public _IListener4_37(KnownClassesCollector _enclosing, Collection4 classes)
			{
				this._enclosing = _enclosing;
				this.classes = classes;
			}

			public void OnEvent(object addedClass)
			{
				this._enclosing.CollectKnownClass(classes, ((IReflectClass)addedClass));
			}

			private readonly KnownClassesCollector _enclosing;

			private readonly Collection4 classes;
		}

		private void CollectKnownClasses(Collection4 collector, IEnumerator knownClasses)
		{
			while (knownClasses.MoveNext())
			{
				IReflectClass clazz = (IReflectClass)knownClasses.Current;
				CollectKnownClass(collector, clazz);
			}
		}

		private void CollectKnownClass(Collection4 classes, IReflectClass clazz)
		{
			if (IsInternalClass(clazz))
			{
				return;
			}
			if (!HasIdentity(clazz))
			{
				return;
			}
			if (clazz.IsArray())
			{
				return;
			}
			classes.Add(clazz);
		}

		private bool IsInternalClass(IReflectClass clazz)
		{
			return _container._handlers.IclassInternal.IsAssignableFrom(clazz);
		}

		private bool HasIdentity(IReflectClass clazz)
		{
			ClassMetadata clazzMeta = _container.ClassMetadataForReflectClass(clazz);
			return clazzMeta == null || clazzMeta.HasIdentity();
		}
	}
}
