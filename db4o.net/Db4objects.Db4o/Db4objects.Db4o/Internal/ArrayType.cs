/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class ArrayType
	{
		public static readonly Db4objects.Db4o.Internal.ArrayType None = new Db4objects.Db4o.Internal.ArrayType
			(0);

		public static readonly Db4objects.Db4o.Internal.ArrayType PlainArray = new Db4objects.Db4o.Internal.ArrayType
			(3);

		public static readonly Db4objects.Db4o.Internal.ArrayType MultidimensionalArray = 
			new Db4objects.Db4o.Internal.ArrayType(4);

		private ArrayType(int value)
		{
			_value = value;
		}

		private readonly int _value;

		public virtual int Value()
		{
			return _value;
		}

		public static Db4objects.Db4o.Internal.ArrayType ForValue(int value)
		{
			switch (value)
			{
				case 0:
				{
					return None;
				}

				case 3:
				{
					return PlainArray;
				}

				case 4:
				{
					return MultidimensionalArray;
				}

				default:
				{
					throw new ArgumentException();
				}
			}
		}
	}
}
