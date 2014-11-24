/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Reflection Array representation.</summary>
	/// <remarks>
	/// Reflection Array representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectArray
	{
		void Analyze(object obj, ArrayInfo info);

		int[] Dimensions(object arr);

		int Flatten(object a_shaped, int[] a_dimensions, int a_currentDimension, object[]
			 a_flat, int a_flatElement);

		object Get(object onArray, int index);

		IReflectClass GetComponentType(IReflectClass a_class);

		int GetLength(object array);

		bool IsNDimensional(IReflectClass a_class);

		object NewInstance(IReflectClass componentType, ArrayInfo info);

		object NewInstance(IReflectClass componentType, int length);

		object NewInstance(IReflectClass componentType, int[] dimensions);

		void Set(object onArray, int index, object element);

		int Shape(object[] a_flat, int a_flatElement, object a_shaped, int[] a_dimensions
			, int a_currentDimension);
	}
}
