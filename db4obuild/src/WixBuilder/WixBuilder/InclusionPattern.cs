using System;
using System.Text.RegularExpressions;

namespace WixBuilder
{
	public class Patterns
	{
		public static System.Predicate<string> Include(string pattern)
		{
			return new InclusionPattern(pattern).Matches;
		}

		public static System.Predicate<string> Exclude(string pattern)
		{
			var include = Include(pattern);
			return s => !include(s);
		}

		public static Predicate<string> And(Predicate<string> lhs, Predicate<string> rhs)
		{
			return s => lhs(s) && rhs(s);
		}

		private class InclusionPattern
		{
			private readonly string _pattern;

			public InclusionPattern(string pattern)
			{
				var re = RegularExpressionBuilder.For(pattern);
				_pattern = ExactMatchPatternFor(re);
			}

			private string ExactMatchPatternFor(string re)
			{
				return "^" + re + "$";
			}

			public bool Matches(string file)
			{
				return Regex.IsMatch(file, _pattern);
			}
		}

	}
}
