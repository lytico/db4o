/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class PreparedArrayContainsComparison : IPreparedComparison
	{
		private readonly ArrayHandler _arrayHandler;

		private readonly IPreparedComparison _preparedComparison;

		private ObjectContainerBase _container;

		public PreparedArrayContainsComparison(IContext context, ArrayHandler arrayHandler
			, ITypeHandler4 typeHandler, object obj)
		{
			_arrayHandler = arrayHandler;
			_preparedComparison = Handlers4.PrepareComparisonFor(typeHandler, context, obj);
			_container = context.Transaction().Container();
		}

		public virtual int CompareTo(object obj)
		{
			// We never expect this call
			// TODO: The callers of this class should be refactored to pass a matcher and
			//       to expect a PreparedArrayComparison.
			throw new InvalidOperationException();
		}

		public virtual bool IsEqual(object array)
		{
			return IsMatch(array, IntMatcher.Zero);
		}

		public virtual bool IsGreaterThan(object array)
		{
			return IsMatch(array, IntMatcher.Positive);
		}

		public virtual bool IsSmallerThan(object array)
		{
			return IsMatch(array, IntMatcher.Negative);
		}

		private bool IsMatch(object array, IntMatcher matcher)
		{
			if (array == null)
			{
				return false;
			}
			IEnumerator i = _arrayHandler.AllElements(_container, array);
			while (i.MoveNext())
			{
				if (matcher.Match(_preparedComparison.CompareTo(i.Current)))
				{
					return true;
				}
			}
			return false;
		}
	}
}
