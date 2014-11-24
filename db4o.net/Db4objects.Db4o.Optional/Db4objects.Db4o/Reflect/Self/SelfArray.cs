/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Self;
using Sharpen;

namespace Db4objects.Db4o.Reflect.Self
{
	public class SelfArray : IReflectArray
	{
		private readonly SelfReflectionRegistry _registry;

		/// <param name="reflector"></param>
		internal SelfArray(IReflector reflector, SelfReflectionRegistry registry)
		{
			_registry = registry;
		}

		public virtual void Analyze(object obj, ArrayInfo info)
		{
		}

		// do nothing
		public virtual int[] Dimensions(object arr)
		{
			return new int[] { GetLength(arr) };
		}

		public virtual int Flatten(object a_shaped, int[] a_dimensions, int a_currentDimension
			, object[] a_flat, int a_flatElement)
		{
			if (a_shaped is object[])
			{
				object[] shaped = (object[])a_shaped;
				System.Array.Copy(shaped, 0, a_flat, 0, shaped.Length);
				return shaped.Length;
			}
			return _registry.FlattenArray(a_shaped, a_flat);
		}

		public virtual object Get(object onArray, int index)
		{
			if (onArray is object[])
			{
				return ((object[])onArray)[index];
			}
			return _registry.GetArray(onArray, index);
		}

		public virtual IReflectClass GetComponentType(IReflectClass a_class)
		{
			return ((SelfClass)a_class).GetComponentType();
		}

		public virtual int GetLength(object array)
		{
			if (array is object[])
			{
				return ((object[])array).Length;
			}
			return _registry.ArrayLength(array);
		}

		public virtual bool IsNDimensional(IReflectClass a_class)
		{
			return false;
		}

		public virtual object NewInstance(IReflectClass componentType, ArrayInfo info)
		{
			// TODO: implement multidimensional arrays.
			int length = info.ElementCount();
			return NewInstance(componentType, length);
		}

		public virtual object NewInstance(IReflectClass componentType, int length)
		{
			return _registry.ArrayFor(((SelfClass)componentType).GetJavaClass(), length);
		}

		public virtual object NewInstance(IReflectClass componentType, int[] dimensions)
		{
			return NewInstance(componentType, dimensions[0]);
		}

		public virtual void Set(object onArray, int index, object element)
		{
			if (onArray is object[])
			{
				((object[])onArray)[index] = element;
				return;
			}
			_registry.SetArray(onArray, index, element);
		}

		public virtual int Shape(object[] a_flat, int a_flatElement, object a_shaped, int
			[] a_dimensions, int a_currentDimension)
		{
			if (a_shaped is object[])
			{
				object[] shaped = (object[])a_shaped;
				System.Array.Copy(a_flat, 0, shaped, 0, a_flat.Length);
				return a_flat.Length;
			}
			return _registry.ShapeArray(a_flat, a_shaped);
		}
	}
}
