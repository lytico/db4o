/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Iterator primitives (concat, map, reduce, filter, etc...).</summary>
	/// <remarks>Iterator primitives (concat, map, reduce, filter, etc...).</remarks>
	/// <exclude></exclude>
	public partial class Iterators
	{
		/// <summary>
		/// Constant indicating that the current element in a
		/// <see cref="Map(IEnumerator, IFunction4)">Map(IEnumerator, IFunction4)</see>
		/// operation
		/// should be skipped.
		/// </summary>
		public static readonly object Skip = new object();

		private sealed class _IEnumerator_20 : IEnumerator
		{
			public _IEnumerator_20()
			{
			}

			public object Current
			{
				get
				{
					throw new InvalidOperationException();
				}
			}

			public bool MoveNext()
			{
				return false;
			}

			public void Reset()
			{
			}
		}

		public static readonly IEnumerator EmptyIterator = new _IEnumerator_20();

		private sealed class _IEnumerable_34 : IEnumerable
		{
			public _IEnumerable_34()
			{
			}

			// do nothing
			public IEnumerator GetEnumerator()
			{
				return Iterators.EmptyIterator;
			}
		}

		public static readonly IEnumerable EmptyIterable = new _IEnumerable_34();

		internal static readonly object NoElement = new object();

		/// <summary>
		/// Generates
		/// <see cref="Tuple">Tuple</see>
		/// items with indexes starting at 0.
		/// </summary>
		/// <param name="iterable">the iterable to be enumerated</param>
		public static IEnumerable Enumerate(IEnumerable iterable)
		{
			return new _IEnumerable_48(iterable);
		}

		private sealed class _IEnumerable_48 : IEnumerable
		{
			public _IEnumerable_48(IEnumerable iterable)
			{
				this.iterable = iterable;
			}

			public IEnumerator GetEnumerator()
			{
				return new EnumerateIterator(iterable.GetEnumerator());
			}

			private readonly IEnumerable iterable;
		}

		public static bool Any(IEnumerator iterator, IPredicate4 condition)
		{
			while (iterator.MoveNext())
			{
				if (condition.Match(iterator.Current))
				{
					return true;
				}
			}
			return false;
		}

		public static IEnumerator Concat(IEnumerator[] array)
		{
			return Concat(Iterate((object[])array));
		}

		public static IEnumerator Concat(IEnumerator iterators)
		{
			return new CompositeIterator4(iterators);
		}

		public static IEnumerable Concat(IEnumerable[] iterables)
		{
			return Concat(Iterable(iterables));
		}

		public static IEnumerable Concat(IEnumerable iterables)
		{
			return new CompositeIterable4(iterables);
		}

		public static IEnumerator Concat(IEnumerator first, IEnumerator second)
		{
			return Concat(new IEnumerator[] { first, second });
		}

		public static IEnumerable ConcatMap(IEnumerable iterable, IFunction4 function)
		{
			return Concat(Map(iterable, function));
		}

		/// <summary>
		/// Returns a new iterator which yields the result of applying the function
		/// to every element in the original iterator.
		/// </summary>
		/// <remarks>
		/// Returns a new iterator which yields the result of applying the function
		/// to every element in the original iterator.
		/// <see cref="Skip">Skip</see>
		/// can be returned from function to indicate the current
		/// element should be skipped.
		/// </remarks>
		/// <param name="iterator"></param>
		/// <param name="function"></param>
		/// <returns></returns>
		public static IEnumerator Map(IEnumerator iterator, IFunction4 function)
		{
			return new FunctionApplicationIterator(iterator, function);
		}

		public static IEnumerator Map(object[] array, IFunction4 function)
		{
			return Map(new ArrayIterator4(array), function);
		}

		public static IEnumerator Filter(object[] array, IPredicate4 predicate)
		{
			return Filter(new ArrayIterator4(array), predicate);
		}

		public static IEnumerable Filter(IEnumerable source, IPredicate4 predicate)
		{
			return new _IEnumerable_112(source, predicate);
		}

		private sealed class _IEnumerable_112 : IEnumerable
		{
			public _IEnumerable_112(IEnumerable source, IPredicate4 predicate)
			{
				this.source = source;
				this.predicate = predicate;
			}

			public IEnumerator GetEnumerator()
			{
				return Iterators.Filter(source.GetEnumerator(), predicate);
			}

			private readonly IEnumerable source;

			private readonly IPredicate4 predicate;
		}

		public static IEnumerator Filter(IEnumerator iterator, IPredicate4 predicate)
		{
			return new FilteredIterator(iterator, predicate);
		}

		public static IEnumerable SingletonIterable(object element)
		{
			return new _IEnumerable_124(element);
		}

		private sealed class _IEnumerable_124 : IEnumerable
		{
			public _IEnumerable_124(object element)
			{
				this.element = element;
			}

			public IEnumerator GetEnumerator()
			{
				return Iterators.SingletonIterator(element);
			}

			private readonly object element;
		}

		public static IEnumerable Append(IEnumerable front, object last)
		{
			return Concat(Iterable(new object[] { front, SingletonIterable(last) }));
		}

		public static IEnumerator Iterator(IEnumerable iterable)
		{
			return iterable.GetEnumerator();
		}

		public static IEnumerator Iterate(object[] array)
		{
			return new ArrayIterator4(array);
		}

		public static IEnumerator Revert(IEnumerator iterator)
		{
			iterator.Reset();
			List4 tail = null;
			while (iterator.MoveNext())
			{
				tail = new List4(tail, iterator.Current);
			}
			return Iterate(tail);
		}

		public static IEnumerator Iterate(List4 list)
		{
			if (list == null)
			{
				return EmptyIterator;
			}
			Collection4 collection = new Collection4();
			while (list != null)
			{
				collection.Add(list._element);
				list = ((List4)list._next);
			}
			return collection.GetEnumerator();
		}

		public static int Size(IEnumerable iterable)
		{
			return Size(iterable.GetEnumerator());
		}

		public static object Next(IEnumerator iterator)
		{
			if (!iterator.MoveNext())
			{
				throw new InvalidOperationException();
			}
			return iterator.Current;
		}

		public static int Size(IEnumerator iterator)
		{
			int count = 0;
			while (iterator.MoveNext())
			{
				++count;
			}
			return count;
		}

		public static string ToString(IEnumerable i)
		{
			return ToString(i.GetEnumerator());
		}

		public static string ToString(IEnumerator i)
		{
			return Join(i, "[", "]", ", ");
		}

		public static string Join(IEnumerable i, string separator)
		{
			return Join(i.GetEnumerator(), separator);
		}

		public static string Join(IEnumerator i, string separator)
		{
			return Join(i, string.Empty, string.Empty, separator);
		}

		public static string Join(IEnumerator i, string prefix, string suffix, string separator
			)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(prefix);
			if (i.MoveNext())
			{
				sb.Append(i.Current);
				while (i.MoveNext())
				{
					sb.Append(separator);
					sb.Append(i.Current);
				}
			}
			sb.Append(suffix);
			return sb.ToString();
		}

		public static object[] ToArray(IEnumerator tests)
		{
			return ToArray(tests, new _IArrayFactory_230());
		}

		private sealed class _IArrayFactory_230 : IArrayFactory
		{
			public _IArrayFactory_230()
			{
			}

			public object[] NewArray(int size)
			{
				return new object[size];
			}
		}

		public static object[] ToArray(IEnumerator tests, IArrayFactory factory)
		{
			Collection4 elements = new Collection4(tests);
			return elements.ToArray(factory.NewArray(elements.Size()));
		}

		/// <summary>Yields a flat sequence of elements.</summary>
		/// <remarks>
		/// Yields a flat sequence of elements. Any
		/// <see cref="IEnumerable">IEnumerable</see>
		/// or
		/// <see cref="IEnumerator">IEnumerator</see>
		/// found in the original sequence is recursively flattened.
		/// </remarks>
		/// <param name="iterator">original sequence</param>
		public static IEnumerator Flatten(IEnumerator iterator)
		{
			return new FlatteningIterator(iterator);
		}

		public static IEnumerable Map(IEnumerable iterable, IFunction4 function)
		{
			return new _IEnumerable_253(iterable, function);
		}

		private sealed class _IEnumerable_253 : IEnumerable
		{
			public _IEnumerable_253(IEnumerable iterable, IFunction4 function)
			{
				this.iterable = iterable;
				this.function = function;
			}

			public IEnumerator GetEnumerator()
			{
				return Iterators.Map(iterable.GetEnumerator(), function);
			}

			private readonly IEnumerable iterable;

			private readonly IFunction4 function;
		}

		public static IEnumerable CrossProduct(IEnumerable iterables)
		{
			return CrossProduct((IEnumerable[])ToArray(iterables.GetEnumerator(), new _IArrayFactory_261
				()));
		}

		private sealed class _IArrayFactory_261 : IArrayFactory
		{
			public _IArrayFactory_261()
			{
			}

			public object[] NewArray(int size)
			{
				return new IEnumerable[size];
			}
		}

		public static IEnumerable CrossProduct(IEnumerable[] iterables)
		{
			return CrossProduct(iterables, 0, Iterators.EmptyIterable);
		}

		private static IEnumerable CrossProduct(IEnumerable[] iterables, int level, IEnumerable
			 row)
		{
			if (level == iterables.Length - 1)
			{
				return Map(iterables[level], new _IFunction4_276(row));
			}
			return ConcatMap(iterables[level], new _IFunction4_284(iterables, level, row));
		}

		private sealed class _IFunction4_276 : IFunction4
		{
			public _IFunction4_276(IEnumerable row)
			{
				this.row = row;
			}

			public object Apply(object arg)
			{
				return Iterators.Append(row, arg);
			}

			private readonly IEnumerable row;
		}

		private sealed class _IFunction4_284 : IFunction4
		{
			public _IFunction4_284(IEnumerable[] iterables, int level, IEnumerable row)
			{
				this.iterables = iterables;
				this.level = level;
				this.row = row;
			}

			public object Apply(object arg)
			{
				return Iterators.CrossProduct(iterables, level + 1, Iterators.Append(row, arg));
			}

			private readonly IEnumerable[] iterables;

			private readonly int level;

			private readonly IEnumerable row;
		}

		public static IEnumerable Iterable(object[] objects)
		{
			return new _IEnumerable_292(objects);
		}

		private sealed class _IEnumerable_292 : IEnumerable
		{
			public _IEnumerable_292(object[] objects)
			{
				this.objects = objects;
			}

			public IEnumerator GetEnumerator()
			{
				return Iterators.Iterate(objects);
			}

			private readonly object[] objects;
		}

		public static IEnumerator SingletonIterator(object element)
		{
			return new SingleValueIterator(element);
		}

		public static IEnumerable Iterable(IEnumerator iterator)
		{
			return new _IEnumerable_304(iterator);
		}

		private sealed class _IEnumerable_304 : IEnumerable
		{
			public _IEnumerable_304(IEnumerator iterator)
			{
				this.iterator = iterator;
			}

			public IEnumerator GetEnumerator()
			{
				return iterator;
			}

			private readonly IEnumerator iterator;
		}

		public static IEnumerator Copy(IEnumerator iterator)
		{
			return new Collection4(iterator).GetEnumerator();
		}

		public static IEnumerator Take(int count, IEnumerator iterator)
		{
			return new _IEnumerator_316(count, iterator);
		}

		private sealed class _IEnumerator_316 : IEnumerator
		{
			public _IEnumerator_316(int count, IEnumerator iterator)
			{
				this.count = count;
				this.iterator = iterator;
				this._taken = 0;
			}

			private int _taken;

			public object Current
			{
				get
				{
					if (this._taken > count)
					{
						throw new InvalidOperationException();
					}
					return iterator.Current;
				}
			}

			public bool MoveNext()
			{
				if (this._taken < count)
				{
					if (!iterator.MoveNext())
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
				throw new NotImplementedException();
			}

			private readonly int count;

			private readonly IEnumerator iterator;
		}

		public static IEnumerator Range(int fromInclusive, int toExclusive)
		{
			if (toExclusive < fromInclusive)
			{
				throw new ArgumentException();
			}
			return Take(toExclusive - fromInclusive, Series(fromInclusive - 1, new _IFunction4_350
				()).GetEnumerator());
		}

		private sealed class _IFunction4_350 : IFunction4
		{
			public _IFunction4_350()
			{
			}

			public object Apply(object i)
			{
				return (((int)i)) + 1;
			}
		}

		public static IEnumerable Series(object seed, IFunction4 function)
		{
			return new _IEnumerable_356(seed, function);
		}

		private sealed class _IEnumerable_356 : IEnumerable
		{
			public _IEnumerable_356(object seed, IFunction4 function)
			{
				this.seed = seed;
				this.function = function;
			}

			public IEnumerator GetEnumerator()
			{
				return new _IEnumerator_358(seed, function);
			}

			private sealed class _IEnumerator_358 : IEnumerator
			{
				public _IEnumerator_358(object seed, IFunction4 function)
				{
					this.seed = seed;
					this.function = function;
					this._current = seed;
				}

				private object _current;

				public object Current
				{
					get
					{
						return this._current;
					}
				}

				public bool MoveNext()
				{
					this._current = function.Apply(this._current);
					return true;
				}

				public void Reset()
				{
					this._current = seed;
				}

				private readonly object seed;

				private readonly IFunction4 function;
			}

			private readonly object seed;

			private readonly IFunction4 function;
		}
	}
}
