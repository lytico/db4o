/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	internal class ObjectAnalyzer
	{
		private readonly ObjectContainerBase _container;

		private readonly object _obj;

		private Db4objects.Db4o.Internal.ClassMetadata _classMetadata;

		private Db4objects.Db4o.Internal.ObjectReference _ref;

		private bool _notStorable;

		internal ObjectAnalyzer(ObjectContainerBase container, object obj)
		{
			_container = container;
			_obj = obj;
		}

		internal virtual void Analyze(Transaction trans)
		{
			_ref = trans.ReferenceForObject(_obj);
			if (_ref != null)
			{
				_classMetadata = _ref.ClassMetadata();
				return;
			}
			IReflectClass claxx = _container.Reflector().ForObject(_obj);
			if (claxx == null)
			{
				NotStorable(_obj, claxx);
				return;
			}
			if (!DetectClassMetadata(trans, claxx))
			{
				return;
			}
			if (IsValueType(_classMetadata))
			{
				NotStorable(_obj, _classMetadata.ClassReflector(), " Value types can only be stored embedded in parent objects."
					);
			}
		}

		private bool DetectClassMetadata(Transaction trans, IReflectClass claxx)
		{
			_classMetadata = _container.GetActiveClassMetadata(claxx);
			if (_classMetadata != null)
			{
				if (!_classMetadata.IsStorable())
				{
					NotStorable(_obj, claxx);
					return false;
				}
				return true;
			}
			_classMetadata = _container.ProduceClassMetadata(claxx);
			if (_classMetadata == null || !_classMetadata.IsStorable())
			{
				NotStorable(_obj, claxx);
				return false;
			}
			// The following may return a reference if the object is held
			// in a static variable somewhere ( often: Enums) that gets
			// stored or associated on initialization of the ClassMetadata.
			_ref = trans.ReferenceForObject(_obj);
			return true;
		}

		private void NotStorable(object obj, IReflectClass claxx)
		{
			NotStorable(obj, claxx, null);
		}

		private void NotStorable(object obj, IReflectClass claxx, string message)
		{
			_container.NotStorable(claxx, obj, message);
			_notStorable = true;
		}

		internal virtual bool NotStorable()
		{
			return _notStorable;
		}

		private bool IsValueType(Db4objects.Db4o.Internal.ClassMetadata classMetadata)
		{
			return classMetadata.IsValueType();
		}

		internal virtual Db4objects.Db4o.Internal.ObjectReference ObjectReference()
		{
			return _ref;
		}

		public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _classMetadata;
		}
	}
}
