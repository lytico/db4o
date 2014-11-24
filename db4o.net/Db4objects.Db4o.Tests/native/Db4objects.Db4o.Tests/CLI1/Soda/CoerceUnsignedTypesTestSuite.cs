using System;
using System.Reflection;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	public class CoerceUnsignedTypesTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override Type[] TestUnits()
		{
			return new[] { typeof (CoerceUnsignedTypesTestUnit) };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new[]
			       	{
			       		new Db4oFixtureProvider(),
						TestVariables.FieldTypeFixtureProvider,
			       	};
		}
	}

	public class TestVariables
	{
		public static readonly FixtureVariable Types = new FixtureVariable("Types");
		
		public static readonly IFixtureProvider FieldTypeFixtureProvider = new SimpleFixtureProvider(
				Types,
				new[]
				{
					new TypeSpec(typeof(ushort), ushort.MinValue, ushort.MaxValue),
					new TypeSpec(typeof(byte), byte.MinValue, byte.MaxValue),
					new TypeSpec(typeof(uint), uint.MinValue, uint.MaxValue),
					new TypeSpec(typeof(ulong), ulong.MinValue, ulong.MaxValue),
					
					new TypeSpec(typeof(sbyte), sbyte.MinValue, sbyte.MaxValue, sbyte.MinValue - 1),
					new TypeSpec(typeof(short), short.MinValue, short.MaxValue, short.MinValue - 1),
					new TypeSpec(typeof(int), int.MinValue, int.MaxValue, (long) int.MinValue - 1),
					new TypeSpec(typeof(long), long.MinValue, long.MaxValue, (decimal) long.MinValue - 1),
					
					new TypeSpec(typeof(decimal), decimal.MinValue, decimal.MaxValue, -42)
				});

		public static TypeSpec Current
		{
			get
			{
				return (TypeSpec) Types.Value;
			}
		}
	}

	public class TypeSpec : ILabeled
	{
		public readonly Type Type;
		public readonly object MinValue;
		public readonly object MaxValue;
		public object InvalidValue = -42;

		public TypeSpec(Type type, object minValue, object maxValue, object invalidValue)
		{
			Type = type;
			MinValue = minValue;
			MaxValue = maxValue;
			InvalidValue = invalidValue;
		}

		public TypeSpec(Type type, object minValue, object maxValue) : this(type, minValue, maxValue, -42)
		{
		}

		public string TypeName
		{
			get { return Type.Name; }
		}

		public bool SupportsRangeErrorTests
		{
			get { return Type != typeof(decimal); }
		}

		public object NewMinValue()
		{
			return NewInstance(MinValue);
		}

		public object NewMaxValue()
		{
			return NewInstance(MaxValue);
		}
		
		public object NewInstance(object value)
		{
			var genericItem = typeof(UnsignedItem<>).MakeGenericType(Type);
			var ctor = genericItem.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { Type } , null);
			return ctor.Invoke(new[]{ value });
		}

		public override string ToString()
		{
			return "UnsignedItem<" + Type.Name + ">[" + MinValue + ", "  + MaxValue + "]";
		}

		public string Label()
		{
			return Type.Name;
		}
	}

	public class UnsignedItem<T> where T : struct, IComparable<T>
	{
#if SILVERLIGHT
		public T _value;
#else
		private readonly T _value;
#endif

		public UnsignedItem(T value)
		{
			_value = value;
		}

		public T Value
		{
			get { return _value; }
		}

		public static bool operator==(UnsignedItem<T> left, UnsignedItem<T> right)
		{
			return left.Value.CompareTo(right.Value) == 0;
		}

		public static bool operator !=(UnsignedItem<T> left, UnsignedItem<T> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;

			if (obj.GetType() != typeof(UnsignedItem<T>)) return false;

			var other = (UnsignedItem<T>) obj;

			return other.Value.CompareTo(_value) == 0;
		}
	}
}
