/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericArrayReflector : IReflectArray
	{
		private readonly IReflectArray _delegate;

		public GenericArrayReflector(GenericReflector reflector)
		{
			_delegate = reflector.GetDelegate().Array();
		}

		public virtual void Analyze(object obj, ArrayInfo info)
		{
			_delegate.Analyze(obj, info);
		}

		public virtual int[] Dimensions(object arr)
		{
			return _delegate.Dimensions(arr);
		}

		public virtual int Flatten(object a_shaped, int[] a_dimensions, int a_currentDimension
			, object[] a_flat, int a_flatElement)
		{
			return _delegate.Flatten(a_shaped, a_dimensions, a_currentDimension, a_flat, a_flatElement
				);
		}

		public virtual object Get(object onArray, int index)
		{
			if (onArray is GenericArray)
			{
				return ((GenericArray)onArray)._data[index];
			}
			return _delegate.Get(onArray, index);
		}

		public virtual IReflectClass GetComponentType(IReflectClass claxx)
		{
			claxx = claxx.GetDelegate();
			if (claxx is GenericClass)
			{
				return claxx;
			}
			return _delegate.GetComponentType(claxx);
		}

		public virtual int GetLength(object array)
		{
			if (array is GenericArray)
			{
				return ((GenericArray)array).GetLength();
			}
			return _delegate.GetLength(array);
		}

		public virtual bool IsNDimensional(IReflectClass a_class)
		{
			if (a_class is GenericArrayClass)
			{
				return false;
			}
			return _delegate.IsNDimensional(a_class.GetDelegate());
		}

		public virtual object NewInstance(IReflectClass componentType, ArrayInfo info)
		{
			componentType = componentType.GetDelegate();
			if (componentType is GenericClass)
			{
				int length = info.ElementCount();
				return new GenericArray(((GenericClass)componentType).ArrayClass(), length);
			}
			return _delegate.NewInstance(componentType, info);
		}

		public virtual object NewInstance(IReflectClass componentType, int length)
		{
			componentType = componentType.GetDelegate();
			if (componentType is GenericClass)
			{
				return new GenericArray(((GenericClass)componentType).ArrayClass(), length);
			}
			return _delegate.NewInstance(componentType, length);
		}

		public virtual object NewInstance(IReflectClass componentType, int[] dimensions)
		{
			return _delegate.NewInstance(componentType.GetDelegate(), dimensions);
		}

		public virtual void Set(object onArray, int index, object element)
		{
			if (onArray is GenericArray)
			{
				((GenericArray)onArray)._data[index] = element;
				return;
			}
			_delegate.Set(onArray, index, element);
		}

		public virtual int Shape(object[] a_flat, int a_flatElement, object a_shaped, int
			[] a_dimensions, int a_currentDimension)
		{
			return _delegate.Shape(a_flat, a_flatElement, a_shaped, a_dimensions, a_currentDimension
				);
		}
	}
}
