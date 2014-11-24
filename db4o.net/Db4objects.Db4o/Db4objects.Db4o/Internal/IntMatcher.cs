/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class IntMatcher
	{
		public abstract bool Match(int i);

		private sealed class _IntMatcher_13 : IntMatcher
		{
			public _IntMatcher_13()
			{
			}

			public override bool Match(int i)
			{
				return i == 0;
			}
		}

		public static readonly IntMatcher Zero = new _IntMatcher_13();

		private sealed class _IntMatcher_19 : IntMatcher
		{
			public _IntMatcher_19()
			{
			}

			public override bool Match(int i)
			{
				return i > 0;
			}
		}

		public static readonly IntMatcher Positive = new _IntMatcher_19();

		private sealed class _IntMatcher_25 : IntMatcher
		{
			public _IntMatcher_25()
			{
			}

			public override bool Match(int i)
			{
				return i < 0;
			}
		}

		public static readonly IntMatcher Negative = new _IntMatcher_25();
	}
}
