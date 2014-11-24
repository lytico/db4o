/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Linq
{
	public class QueryOptimizationException : Exception
	{
		internal QueryOptimizationException()
		{
		}

		internal QueryOptimizationException(string message)
			: base(message)
		{
		}

		internal QueryOptimizationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
