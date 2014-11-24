/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit.Data;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Data
{
	public partial class Generators
	{
		public static IEnumerable ArbitraryValuesOf(Type type)
		{
			IEnumerable platformSpecific = PlatformSpecificArbitraryValuesOf(type);
			if (null != platformSpecific)
			{
				return platformSpecific;
			}
			if (type == typeof(int))
			{
				return Take(10, Streams.RandomIntegers());
			}
			if (type == typeof(string))
			{
				return Take(10, Streams.RandomStrings());
			}
			throw new NotImplementedException("No generator for type " + type);
		}

		internal static IEnumerable Trace(IEnumerable source)
		{
			return Iterators.Map(source, new _IFunction4_32());
		}

		private sealed class _IFunction4_32 : IFunction4
		{
			public _IFunction4_32()
			{
			}

			public object Apply(object value)
			{
				Sharpen.Runtime.Out.WriteLine(value);
				return value;
			}
		}

		public static IEnumerable Take(int count, IEnumerable source)
		{
			return new _IEnumerable_41(source, count);
		}

		private sealed class _IEnumerable_41 : IEnumerable
		{
			public _IEnumerable_41(IEnumerable source, int count)
			{
				this.source = source;
				this.count = count;
			}

			public IEnumerator GetEnumerator()
			{
				return new _IEnumerator_43(source, count);
			}

			private sealed class _IEnumerator_43 : IEnumerator
			{
				public _IEnumerator_43(IEnumerable source, int count)
				{
					this.source = source;
					this.count = count;
					this._taken = 0;
					this._delegate = source.GetEnumerator();
				}

				private int _taken;

				private IEnumerator _delegate;

				public object Current
				{
					get
					{
						if (this._taken > count)
						{
							throw new InvalidOperationException();
						}
						return this._delegate.Current;
					}
				}

				public bool MoveNext()
				{
					if (this._taken < count)
					{
						if (!this._delegate.MoveNext())
						{
							this._taken = count;
							return false;
						}
						++this._taken;
						return true;
					}
					return false;
				}

				public void Reset()
				{
					this._taken = 0;
					this._delegate = source.GetEnumerator();
				}

				private readonly IEnumerable source;

				private readonly int count;
			}

			private readonly IEnumerable source;

			private readonly int count;
		}
	}
}
