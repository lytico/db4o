/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <summary>n-dimensional array</summary>
	/// <exclude></exclude>
	public class MultidimensionalArrayHandler : ArrayHandler
	{
		public MultidimensionalArrayHandler(ITypeHandler4 a_handler, bool a_isPrimitive) : 
			base(a_handler, a_isPrimitive)
		{
		}

		public MultidimensionalArrayHandler()
		{
		}

		// required for reflection cloning
		public sealed override IEnumerator AllElements(ObjectContainerBase container, object
			 array)
		{
			return AllElementsMultidimensional(ArrayReflector(container), array);
		}

		public static IEnumerator AllElementsMultidimensional(IReflectArray reflectArray, 
			object array)
		{
			return new MultidimensionalArrayIterator(reflectArray, (object[])array);
		}

		protected static int ElementCount(int[] a_dim)
		{
			int elements = a_dim[0];
			for (int i = 1; i < a_dim.Length; i++)
			{
				elements = elements * a_dim[i];
			}
			return elements;
		}

		public sealed override byte Identifier()
		{
			return Const4.Yaparrayn;
		}

		protected override ArrayInfo NewArrayInfo()
		{
			return new MultidimensionalArrayInfo();
		}

		protected override void ReadDimensions(ArrayInfo info, IReadBuffer buffer)
		{
			ReadDimensions(info, buffer, buffer.ReadInt());
		}

		private void ReadDimensions(ArrayInfo info, IReadBuffer buffer, int dimensionCount
			)
		{
			int[] dim = new int[dimensionCount];
			for (int i = 0; i < dim.Length; i++)
			{
				dim[i] = buffer.ReadInt();
			}
			((MultidimensionalArrayInfo)info).Dimensions(dim);
			info.ElementCount(ElementCount(dim));
		}

		protected override void ReadElements(IReadContext context, ArrayInfo info, object
			 array)
		{
			if (array == null)
			{
				return;
			}
			object[] objects = new object[info.ElementCount()];
			ReadInto(context, info, objects);
			ArrayReflector(Container(context)).Shape(objects, 0, array, ((MultidimensionalArrayInfo
				)info).Dimensions(), 0);
		}

		protected override void WriteDimensions(IWriteContext context, ArrayInfo info)
		{
			int[] dim = ((MultidimensionalArrayInfo)info).Dimensions();
			context.WriteInt(dim.Length);
			for (int i = 0; i < dim.Length; i++)
			{
				context.WriteInt(dim[i]);
			}
		}

		protected override void WriteElements(IWriteContext context, object obj, ArrayInfo
			 info)
		{
			IEnumerator objects = AllElements(Container(context), obj);
			if (HasNullBitmap(info))
			{
				BitMap4 nullBitMap = new BitMap4(info.ElementCount());
				IReservedBuffer nullBitMapBuffer = context.Reserve(nullBitMap.MarshalledLength());
				int currentElement = 0;
				while (objects.MoveNext())
				{
					object current = objects.Current;
					if (current == null)
					{
						nullBitMap.SetTrue(currentElement);
					}
					else
					{
						context.WriteObject(DelegateTypeHandler(), current);
					}
					currentElement++;
				}
				nullBitMapBuffer.WriteBytes(nullBitMap.Bytes());
			}
			else
			{
				while (objects.MoveNext())
				{
					context.WriteObject(DelegateTypeHandler(), objects.Current);
				}
			}
		}

		protected override void AnalyzeDimensions(ObjectContainerBase container, object obj
			, ArrayInfo info)
		{
			int[] dim = ArrayReflector(container).Dimensions(obj);
			((MultidimensionalArrayInfo)info).Dimensions(dim);
			info.ElementCount(ElementCount(dim));
		}

		public override ITypeHandler4 UnversionedTemplate()
		{
			return new Db4objects.Db4o.Internal.Handlers.Array.MultidimensionalArrayHandler();
		}
	}
}
