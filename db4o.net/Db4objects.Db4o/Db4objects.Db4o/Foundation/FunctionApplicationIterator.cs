/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class FunctionApplicationIterator : MappingIterator
	{
		private readonly IFunction4 _function;

		public FunctionApplicationIterator(IEnumerator iterator, IFunction4 function) : base
			(iterator)
		{
			if (null == function)
			{
				throw new ArgumentNullException();
			}
			_function = function;
		}

		protected override object Map(object current)
		{
			return _function.Apply(current);
		}
	}
}
