using System;
using System.Collections.Generic;
using System.Linq;

namespace NullableArraysTest
{
	class NullableHolder<T> where T : struct
	{
		internal T?[] _nullables;
		internal IList<T?> _listOfNullables;

		internal NullableHolder(T?[] nullables)
		{
			_nullables = nullables;
			_listOfNullables = new List<T?>(nullables);
		}

		public static bool operator ==(NullableHolder<T> lhs, NullableHolder<T> rhs)
		{
			object objlhs = lhs;
			object objrhs = rhs;
			if (objlhs != objrhs && (objlhs == null || objrhs == null)) return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(NullableHolder<T> lhs, NullableHolder<T> rhs)
		{
			return !(lhs == rhs);
		}

		private bool Equals(NullableHolder<T> other)
		{
			if (!Compare(_nullables, other._nullables)) return false;
			if (!Compare(_listOfNullables, other._listOfNullables)) return false;

			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != GetType()) return false;

			NullableHolder<T> other = (NullableHolder<T>)obj;

			return Equals(other);
		}

		private static bool Compare<R>(IEnumerable<R> expected, IEnumerable<R> actual)
		{
			if (expected != actual && (actual == null || expected == null)) return false;

			var diff = expected.Except(actual);
			return diff.Count() == 0;
		}

		public override string ToString()
		{
			Func<IEnumerable<T?>, string> toString = e =>
			{
				if (e == null) return "null";
				if (e.Count() == 0) return "empty";
				return e.Aggregate(
					"",
					(acc, next) => acc + ", " + (!next.HasValue ? "null" : next.ToString()),
					result => result.Remove(0, 2));
			};

			string typeName = typeof(T).Name;
			return String.Format(
					"NullableHolder\r\n" +
					"{{\r\n" +
						"\t{0}?[]     : {1}\r\n" +
						"\tIList<{0}?>: {2}\r\n" +
					"}}", typeName, toString(_nullables), toString(_listOfNullables));

		}
	}
}
