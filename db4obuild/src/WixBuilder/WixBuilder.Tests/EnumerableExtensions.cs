using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace WixBuilder.Tests
{
	internal static class EnumerableExtensions
	{
		public static T AssertSingle<T>(this IEnumerable<T> source)
		{
			Assert.IsNotNull(source);

			var enumerator = source.GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext(), "Sequence contains no element");
			var first = enumerator.Current;
			if (!enumerator.MoveNext())
				return first;
			Assert.Fail("Unexpected element: {0}, ...", enumerator.Current);
			// dead code
			return default(T);
		}

		public static string MakeString<T>(this IEnumerable<T> source, string separator)
		{
			var enumerator = source.GetEnumerator();
			if (!enumerator.MoveNext())
				return string.Empty;

			StringBuilder builder = new StringBuilder();
			builder.Append(enumerator.Current);
			while (enumerator.MoveNext())
			{
				builder.Append(separator);
				builder.Append(enumerator.Current);
			}
			return builder.ToString();
		}
	}
}
